using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using alg_Simulation_Evolution.Data;
using alg_Simulation_Evolution.Organisms;
using alg_Simulation_Evolution.Services;

namespace alg_Simulation_Evolution.EngineOfEvolution
{
    public class Evolutioner
    {
        private readonly DataProvider _dataProvider;
        private readonly Canvas _canvas;

        public Evolutioner(DataProvider dataProvider, Canvas canvas)
        {
            _dataProvider = dataProvider;
            _canvas = canvas;

            Starting();
        }

        public async void Starting()
        {
            while (true)
            {
                await Task.Run(() => EvolutionControllerProvider.Continue());

                await foreach (var tuple in Evolving())
                {
                    await Task.Run(() => Thread.Sleep(EvolutionControllerProvider.Delay / 1000)); // 1000 - подобранное значение, оптимального для визуального восприятия
                    if (tuple.Item1.Position != tuple.Item2) tuple.Item1.MoveOnCanvas(tuple.Item2);
                }

            }
        }

        /// <summary> Найти расстояние между двумя позициями </summary>
        /// <param name="position1"> Позиция 1 </param>
        /// <param name="position2"> Позиция 2 </param>
        public static double GetDistanceBetweenPosition(Point position1, Point position2)
        {
            return Math.Sqrt(Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2));
        }

        /// <summary> Эволюционировать: двигать все живые организмы к пище </summary>
        private async IAsyncEnumerable<(IOrganism, Point)> Evolving()
        {
            if (_canvas.Children.Count > _dataProvider.Organisms.Count + _dataProvider.Predators.Count + _dataProvider.Food.Count) yield break;
            foreach (var organism in _dataProvider.Organisms.ToList())
            {
                var (positionNearestFood, distanceNearestFood) = FindNearestFood(organism, _dataProvider.Food);
                if (positionNearestFood != null)
                {
                    var nextPosition = GetNextPosition(organism, (Point) positionNearestFood, distanceNearestFood);
                    yield return (organism, nextPosition);
                }
            }

            if (_canvas.Children.Count > _dataProvider.Organisms.Count + _dataProvider.Predators.Count + _dataProvider.Food.Count) yield break;
            foreach (var predator in _dataProvider.Predators.ToList())
            {
                var (positionNearestFood, distanceNearestFood) = 
                    FindNearestFood(predator, 
                                    _dataProvider.Food, 
                                    _dataProvider.Organisms.Select(x => (IFood) x), 
                                    _dataProvider.Predators.Select(x => (IFood) x));
                if (positionNearestFood != null)
                {
                    var nextPosition = GetNextPosition(predator, (Point) positionNearestFood, distanceNearestFood);
                    yield return (predator, nextPosition);
                }
            }
        }

        /// <summary> Получить следующую позицию для передвижения </summary>
        /// <param name="organism"> Организм </param>
        /// <param name="positionNearestFood"> Позиция ближайшей пищи </param>
        /// <param name="distanceNearestFood"> Расстояние до ближайшей пищи </param>
        private Point GetNextPosition(IOrganism organism, Point positionNearestFood, double distanceNearestFood)
        {
            var shiftSize = GetShiftSize(organism);
            var x = organism.Position.X + shiftSize * (positionNearestFood.X - organism.Position.X) / distanceNearestFood;
            var y = organism.Position.Y + shiftSize * (positionNearestFood.Y - organism.Position.Y) / distanceNearestFood;
            CorrectPositionsOnCanvas(ref x, ref y);

            return new Point(x, y);
        }

        /// <summary> Скорректировать позиции на холсте </summary>
        /// <param name="x"> Координата по X </param>
        /// <param name="y"> Координата по Y </param>
        private void CorrectPositionsOnCanvas(ref double x, ref double y)
        {
            if (x > _canvas.ActualWidth * 0.88)
            {
                x = _canvas.ActualWidth * 0.88;
            }
            else if (x < 10)
            {
                x = 10;
            }

            if (y > _canvas.ActualHeight * 0.88)
            {
                y = _canvas.ActualHeight * 0.88;
            }
            else if (y < 10)
            {
                y = 10;
            }
        }

        /// <summary> Получить размер смещения в соответствии со скоростью организма </summary>
        /// <param name="organism"> Организм </param>
        private double GetShiftSize(IOrganism organism)
        {
            return organism.Speed / 27; // 27 - подобранное значение, оптимально для визуального восприятия
        }

        /// <summary>
        /// Найти ближайшую пищу </summary>
        /// <param name="organism"> Организм </param>
        /// <param name="food"> Вся доступная еда </param>
        private (Point?, double) FindNearestFood(IBody organism, params IEnumerable<IFood>[] food)
        {
            Point? nearestPosition = null;
            IFood? nearestFood = null;
            var minDistance = double.MaxValue;
            foreach (var list in food)
            {
                foreach (var f in list)
                {
                    if (!f.Equals(organism))
                    {
                        var currentDistance = GetDistanceBetweenPosition(organism.Position, f.Position);
                        if (currentDistance < minDistance)
                        {
                            minDistance = currentDistance;
                            nearestFood = f;
                            nearestPosition = f.Position;
                        }
                    }
                }
            }

            if (nearestFood != null)
            {
                if (CheckDistanceIsAvailableForAbsorb(organism, nearestFood, minDistance))
                {
                    ToTryToAbsorb(organism, nearestFood);
                }
            }

            return (nearestPosition, minDistance);
        }

        /// <summary> Проверить на доступном ли расстоянии находится организм от организма </summary>
        /// <param name="organism1"> Организм 1 </param>
        /// <param name="organism2"> Организм 2 </param>
        /// <param name="currentDistance"> Текущая дистанция между организмами </param>
        private bool CheckDistanceIsAvailableForAbsorb(IBody organism1, IBody organism2, double currentDistance)
        {
            return currentDistance < organism1.BodySize / 2 + organism2.BodySize / 2;
        }

        /// <summary> Пробовать поглощать </summary>
        /// <param name="body"> Тело, которое хочет поглотить пищу </param>
        /// <param name="food"> Пища для поглощения </param>
        private void ToTryToAbsorb(IBody body, IFood food)
        {
            if (body is IPredator predator)
            {
                if (food is Organism organism)
                {
                    predator.AbsorbOrganism(organism);
                    if (organism is IPredator pr) _dataProvider.Predators.Remove(pr);
                    else _dataProvider.Organisms.Remove(organism);
                } else if (food is Food food2)
                {
                    predator.AbsorbFood(food2);
                    _dataProvider.Food.Remove(food2);
                }
            } else if (body is IOrganism organism)
            {
                if (food is Food food2)
                {
                    organism.AbsorbFood(food2);
                    _dataProvider.Food.Remove(food2);
                }
            }
        }
    }
}
