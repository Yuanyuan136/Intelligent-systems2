using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class GeneticAlgorithm
    {
        readonly int populationsize = 100; // Размер популяции
        readonly int max_iteration = 20; //Максимальное количество итераций
        readonly double mutationrate = 0.3; // Коэффициент мутации
        readonly double delta = 0.0001; //Дельта-правило
        readonly double ln = 0.3; //Лучшая личность
        readonly double pc = 0.6; //Вероятность скрещивания Pc
        Random rnd = new Random((int)DateTime.Now.Ticks);

        public Func<double, double> Func;
        public List<Descendant> Population = new List<Descendant>();

        //функция для минимизации
        private double funct(double x) 
        {
            return Func(x);
        }

        //Инициализация генетического алгоритма
        public void Init() 
        {
            //Создаюм популяцию
            GeneratePopulation();
            //Расчитываем 
            CalcFitness();
        }

        //Работа генетического алгоритма
        public Descendant Work() 
        {
            int i = 0;
            for (; i < max_iteration; i++) // Ограничим кол-во итераций
            {
                Crossing(); // Скерщивание
                Mutation(); //Мутация
                CalcFitness(); //Расчет приспособленности
                Selection(); //Селекция
                if (Math.Abs(Population.First().Y - Population.Last().Y) <= delta) //Расчет разброса полученных потомков (для остановки алгоритма не дожидаясь максимального кол-ва итераций)
                    break;
            }

            //Возращаем минимальное значение
            return Population.First();
        }

        //Генерация популяции
        public void GeneratePopulation() 
        {
            for(int i = 0; i < populationsize; i++) 
            {
                var desc = new Descendant()
                {
                    X = (rnd.NextDouble() - 0.5) * 100000
                };
                Population.Add(desc);
            }
        }

        //Расчет приспособленности
        public void CalcFitness() 
        {
            foreach(var desc in Population) 
            {
                desc.Y = funct(desc.X);
            }
            Population.Sort();
        }

        //Селекция
        public void Selection() 
        {
            //Селекция усечением
            for(int i = (int)(Population.Count * ln); i < Population.Count; i++) 
                Population.RemoveAt(i);
        }

        //Скрещивание
        public void Crossing() 
        {
            var old_size = Population.Count;
            //Пока не доберем нужное кол-во потомков
            while (Population.Count < populationsize) 
            {
                int i = (int)(rnd.NextDouble() * old_size);
                int j = (int)(rnd.NextDouble() * old_size);
                
                //Получаем индексы потомков
                if (pc > rnd.NextDouble()) 
                {
                    var children = CrossingDesc(Population[i], Population[j]);
                    Population.Add(children.Item1);
                    Population.Add(children.Item2);
                }
            }
        }

        //Скрещивание двух потомков с использованием арифметического оператора кроссинговер
        public (Descendant, Descendant) CrossingDesc(Descendant x, Descendant y) 
        {
            var lambda = rnd.NextDouble();
            var cx = new Descendant() { X = lambda * x.X + (1 - lambda) * y.X };
            var cy = new Descendant() { X = lambda * y.X + (1 - lambda) * x.X };
            return (cx, cy);
        }

        //Мутация
        public void Mutation()
        {   
            foreach (var desc in Population) 
            {
                //С некоторой вероятность производим мутацию от -50 до 50
                if (mutationrate > rnd.NextDouble())
                {
                    desc.X += (rnd.NextDouble() - 0.5) * 100;
                }
            }
        }
    }
}
