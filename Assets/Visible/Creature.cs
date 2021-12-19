using Assets.Classes;
using Assets.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Visible
{
    public class Creature : MonoBehaviour
    {
        public Brain Brain { get; set; }
        public Sense[] Senses { get; set; }
        
        private HashSet<string> _inRange = new HashSet<string>();


        public void InitaliseBrain(int genomeLength, int innerNeurons)
        {
            Brain = new Brain(genomeLength, innerNeurons);
        }

        public void InitaliseBrain(Creature parent1, Creature parent2)
        {
            Brain = new Brain(parent1.Brain, parent2.Brain);
        }

       
        public void Act()
        {

        }

        public double GetSuccessScore(Func<double, double, double, double> successCalculator)
        {
            var location = transform.position;

            return successCalculator(location.x, location.y, location.z);
        }

        public bool IsInRange(GameObject obj)
        {
            return obj != null && _inRange.Contains(obj.tag);
        }

        private void Start()
        {
            CreateSenses();
        }

        private void Update()
        {

        }


        void OnDisable()
        {
            _inRange.Clear();
        }

        private void OnTriggerEnter(Collider collider)
        {
            var tag = collider.gameObject.tag;

            if (!collider.gameObject.CompareTag(tag))
            {
                return;
            }

            _inRange.Add(tag);
        }

        private void OnTriggerExit(Collider collider)
        {
            _inRange.Remove(collider.gameObject.tag);
        }

        private void CreateSenses()
        {
            Senses = Enum
                .GetValues(typeof(SenseType))
                .Cast<SenseType>()
                .Select(x => new Sense(x, 0))
                .ToArray();
        }
    }
}
