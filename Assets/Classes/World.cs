using System;
using Assets.Visible;
using System.Linq;
using System.Threading.Tasks;
using Assets.Enums;
using TreeEditor;
using UnityEngine;
using Random = System.Random;

namespace Assets.Classes
{
    public class World : MonoBehaviour
    {
        public GameObject MainCamera;
        public GameObject CreatureTemplate;

        public int GridDepth;
        public int GridWidth;
        public int PopulationSize;
        public int StepsUntilReproduction;
        public double SuccessThreshold;

        public int GenomeLength;
        public int InnerNeurons;

        public Creature[] Creatures;

        private Random _randomNumberGenerator;
        private int _currentStep;

        private bool _started = false;

        private void Start()
        {
            _started = true;

            _randomNumberGenerator = new Random();
            _currentStep = 0;

            SetupWorld();
            CreateCreatures();
        }

        private void Update()
        {
            if (_currentStep % StepsUntilReproduction == 0)
            {
                BreedCreatures();
            }

            Parallel.ForEach(Creatures, creature =>
            {
                ProcessSenses(creature);
                creature.Act();
            });

            _currentStep++;
        }

        private void OnDrawGizmos()
        {
            if (_started)
            {
                return;
            }

            Gizmos.color = Color.red;

            foreach(var creature in Creatures)
            {
                Gizmos.DrawWireCube(creature.transform.position + creature.transform.forward * 10,
                    creature.transform.localScale + creature.transform.forward * 20);
            }
        }

        private void SetupWorld()
        {
            MainCamera.transform.Translate(GridWidth / 2.0f, 0, 0);
        }

        private void CreateCreatures()
        {
            Creatures = new Creature[PopulationSize];

            for (var i = 0; i < PopulationSize; i++)
            {
                var newCreature = Instantiate(CreatureTemplate, transform.position, transform.rotation);
                var creatureComponent = (Creature)newCreature.AddComponent(typeof(Creature));
                creatureComponent.InitaliseBrain(GenomeLength, InnerNeurons);

                Creatures[i] = creatureComponent;

                Collider[] colliders;
                float x, z;
                do
                {
                    x = (float)_randomNumberGenerator.NextDouble() * GridWidth;
                    z = (float)_randomNumberGenerator.NextDouble() * GridDepth;

                    colliders = Physics.OverlapSphere(transform.position + new Vector3(x, 0, z), 1.5f);

                } while (colliders.Length > 0);

                newCreature.transform.Translate(x, 0, z);
            }
        }

        private void ProcessSenses(Creature creature)
        {
            var x = transform.position.x;
            var y = transform.position.y;
            var z = transform.position.z;

            var ahead = transform.position + transform.forward;
            var behind = transform.position - transform.forward;
            var left = transform.position - transform.right;
            var right = transform.position + transform.right;
            var above = transform.position + transform.up;
            var below = transform.position - transform.up;

            var forwardColliders = Physics.OverlapBox(
                creature.transform.position + creature.transform.forward * 50,
                creature.transform.localScale + creature.transform.forward * 100, Quaternion.identity);

            foreach (var sense in creature.Senses)
            {
                switch (sense.SenseType)
                {
                    case SenseType.Age:
                        sense.Intensity = (double)(_currentStep % StepsUntilReproduction) / StepsUntilReproduction;
                        break;
                    case SenseType.BlockageForward:
                        sense.Intensity = forwardColliders
                            .Any(c => Vector3.Distance(transform.position, c.gameObject.transform.position) < 2)
                            ? 0
                            : 1;
                        break;
                    case SenseType.BlockageLeftRight:
                        //sense.Intensity = leftRightColliders
                        //    .Any(c => Vector3.Distance(transform.position, c.gameObject.transform.position) < 2)
                        //    ? 0
                        //    : 1;
                        break;
                    case SenseType.BlockageLongRangeForward:
                        sense.Intensity = forwardColliders
                            .Any(c => Vector3.Distance(transform.position, c.gameObject.transform.position) < 2)
                            ? 0
                            : 1;
                        break;
                    case SenseType.BorderDistanceEastWest:
                        break;
                    case SenseType.BorderDistanceNearest:
                        break;
                    case SenseType.BorderDistanceNorthSouth:
                        break;
                    case SenseType.Damage:
                        break;
                    case SenseType.GeneticSimilarityOfForwardNeighbour:
                        break;
                    case SenseType.HeatGradientForward:
                        break;
                    case SenseType.HeatGradientLeftRight:
                        break;
                    case SenseType.LastMovementX:
                        break;
                    case SenseType.LastMovementY:
                        break;
                    case SenseType.LastMovementZ:
                        break;
                    case SenseType.LightGradientForward:
                        break;
                    case SenseType.LightGradientLeftRight:
                        break;
                    case SenseType.Oscillator:
                        break;
                    case SenseType.PheromoneDensity:
                        break;
                    case SenseType.PheromoneGradientForward:
                        break;
                    case SenseType.PheromoneGradientLeftRight:
                        break;
                    case SenseType.PopulationDensity:
                        break;
                    case SenseType.PopulationGradientForward:
                        break;
                    case SenseType.PopulationGradientLeftRight:
                        break;
                    case SenseType.PopulationLongRangeForward:
                        break;
                    case SenseType.RandomInput:
                        break;
                    case SenseType.Sound:
                        break;
                    case SenseType.Touch:
                        break;
                    case SenseType.WorldLocationEastWest:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void BreedCreatures()
        {
            if (_currentStep == 0)
            {
                return;
            }

            var parents = Creatures
                .Where(creature => creature.GetSuccessScore(SuccessFunction) > SuccessThreshold)
                .ToList();

            parents.Shuffle();

            for (var n = 0; n < PopulationSize; n++)
            {
                Creatures[n] = new Creature();
                Creatures[n].InitaliseBrain(parents[n % parents.Count], parents[(n + 1) % parents.Count]);
            }
        }

        private double SuccessFunction(double x, double y, double z)
        {
            return (z * GridDepth) + x;
        }
    }
}
