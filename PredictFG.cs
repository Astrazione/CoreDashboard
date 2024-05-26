using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;


namespace PredictFinalGrades
{
    class StudentData
    {
        public string StudentId { get; set; }

        public float x1 { get; set; }
        public float x2 { get; set; }
        public float x3 { get; set; }
        public float x4 { get; set; }
        public float x5 { get; set; }
        public float x6 { get; set; }
        public float x7 { get; set; }
        public float x8 { get; set; }
        public float x9 { get; set; }

        [ColumnName("Label")]
        public UInt32 FinalGrade { get; set; } 

    }

    public class ScorePrediction
    {
        [ColumnName("PredictedLabel")]
        public UInt32 PredictFG{ get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            #region Обучающая выборка
            var allLines2 = File.ReadAllLines("C:/Users/One/Desktop/dataset2.csv");
            int length = allLines2.Length;
            int index = 1;

            var studentDataList2 = new List<StudentData>(); //Список студентов

            string controleString = "Основные принципы организации Языка Python. Базовые элементы программирования и типы данных0";

            while (index < length)
            {

                var theme = allLines2[index].Split(";")[3]; //Тема
                var obj = allLines2[index].Split(";")[2].Split(" ")[1][0]; // П или Л (практика или лекция)

                if (theme == controleString & obj == 'П')
                {
                    var start = index;
                    var studentId = allLines2[index].Split(';')[0]; //ID студента

                    var finalGrade = float.Parse(allLines2[index].Split(';')[7]); //Итоговый балл

                    var realFinalGrade = TransformGrade(finalGrade);
                    
                    //Подсчет суммы накопленных баллов за:
                    // 9: 1 контрольная + 8 лаб
                    // 13: 2 контрольные + 11 лаб

                    var scores2 = new float[9];
                    var n = 9;
                    for (int i = start; i < start + n - 1; i++)
                    {
                        index++;

                        var line = allLines2[i].Split(';');
                        if (line[4] != "")
                            scores2[i - start] = float.Parse(line[4], CultureInfo.InvariantCulture);
                        else scores2[i - start] = 0f;
                    }

                    var line2 = allLines2[start + n - 1].Split(';');
                    if (line2[6] != "")
                        scores2[8] = float.Parse(line2[6], CultureInfo.InvariantCulture);
                    else
                        scores2[8] = 0f;


                    studentDataList2.Add(new StudentData
                    {
                        StudentId = studentId,
                        x1 = scores2[0],
                        x2 = scores2[1],
                        x3 = scores2[2],
                        x4 = scores2[3],
                        x5 = scores2[4],
                        x6 = scores2[5],
                        x7 = scores2[6],
                        x8 = scores2[7],
                        x9 = scores2[8],
                        FinalGrade = realFinalGrade
                    });
                }
                index += 1;
            }
            #endregion

            #region Тестовая выборка
            var allLines = File.ReadAllLines("C:/Users/One/Desktop/Данные_ПиОА_2023_1семестр.csv");

            var studentDataList = new List<StudentData>(); //Список студентов
            foreach (var line in allLines)
            {
                var splitLine = line.Split(';');
                if (splitLine[0].Split(" ")[0] == "Студент")
                {
                    float[] scores = new float[9];
                    int[] indexes =
                    {
                        7, 9, 11, 15, 17, 19, 21, 23, 25
                    };

                    var StudentId = splitLine[0];
                    for (int i = 0; i < indexes.Length; i++)
                    {
                        if (splitLine[indexes[i]].Length == 0)
                            scores[i] = 0f;
                        else
                            scores[i] = float.Parse(splitLine[indexes[i]], CultureInfo.InvariantCulture);
                    }

                    var finalGrade = float.Parse(splitLine.ElementAt(61), CultureInfo.InvariantCulture);

                    var realFinalGrade = TransformGrade(finalGrade);
                    var student = new StudentData
                    {
                        StudentId = StudentId,
                        x1 = scores[0],
                        x2 = scores[1],
                        x3 = scores[2],
                        x4 = scores[3],
                        x5 = scores[4],
                        x6 = scores[5],
                        x7 = scores[6],
                        x8 = scores[7],
                        x9 = scores[8],
                        FinalGrade = realFinalGrade
                    };
                    studentDataList.Add(student);
                }
            }
            #endregion

            
            // Преобразование списка в IDataView
            var dataTrain = mlContext.Data.LoadFromEnumerable(studentDataList);
            var dataTest = mlContext.Data.LoadFromEnumerable(studentDataList2);

            var options = new LightGbmMulticlassTrainer.Options()
            {
                MinimumExampleCountPerLeaf = 2,
                ExampleWeightColumnName = "x9", //Контрольная
                NumberOfIterations = 40,
                NumberOfLeaves = 19,
                MaximumBinCountPerFeature = 19,
                LearningRate = 0.7,
                UnbalancedSets = true
            };

            var pipeline2 = mlContext.Transforms
                .Concatenate("Features", "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9")
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(mlContext.MulticlassClassification.Trainers.LightGbm(options))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"))
                .AppendCacheCheckpoint(mlContext);


            // Обучение модели
            var model2 = pipeline2.Fit(dataTrain);


            // Оценка модели
            var predictions = model2.Transform(dataTest);
            Console.WriteLine($"Mean_Accuracy: {GetMetrics(mlContext, studentDataList, model2, true)}");

            Console.WriteLine(new String('-', 30));
            


            var metrics = mlContext.MulticlassClassification.Evaluate(predictions);

/*          // Вывод метрик
            Console.WriteLine($"LogLoss: {Math.Round(metrics.LogLoss, 3)}");
            Console.WriteLine($"LogLossReduction: {Math.Round(metrics.LogLossReduction, 3)}");
            Console.WriteLine($"MacroAccuracy: {Math.Round(metrics.MacroAccuracy, 3)}");
            Console.WriteLine($"MicroAccuracy: {Math.Round(metrics.MicroAccuracy, 3)}");
            var perClassLogLoss = metrics.PerClassLogLoss;
            int ind = 2;
            foreach (var item in perClassLogLoss)
                Console.WriteLine($"{ind++}: {Math.Round(item, 3)}");*/

        }

        public static UInt32 TransformGrade(float grade)
        {
            if (grade >= 91)
                return 5;
            else if (grade >= 76)
                return 4;
            else if (grade >= 61)
                return 3;

            return 2;
        }

        public static void GetPredictions(MLContext mlContext, List<StudentData> studentDataList, ITransformer model)
        {
            // Прогнозирование
            Console.WriteLine(new String('-', 40));
            Console.WriteLine("Блок прогнозирования");

            for (int st = 0; st < studentDataList.Count; st++)
            {
                var finalGradeRS = studentDataList[st].FinalGrade;
                var student = studentDataList[st].StudentId;

                var predictor = mlContext.Model.CreatePredictionEngine<StudentData, ScorePrediction>(model);
                var result = predictor.Predict(studentDataList[st]);

                Console.WriteLine($"Студент: {student}");
                Console.WriteLine($"Реальный итоговый балл: {finalGradeRS}");
                Console.WriteLine($"Прогнозируемый итоговый балл: {result.PredictFG}");

                Console.WriteLine(new String('-', 40));
            }
        }

        public static double GetMetrics(MLContext mlContext, List<StudentData> studentDataList, ITransformer model, bool getMatrix)
        {
            //Будущее распрелеление оценок
            var class2_count = 0;
            var class3_count = 0;
            var class4_count = 0;
            var class5_count = 0;

            //Словарь для правильно предсказанных классов
            Dictionary<int, float> metrics = new Dictionary<int, float>(){
                [5] = 0f, [4] = 0f,
                [3] = 0f, [2] = 0f
            };

            //Заполняем словарь и распределение
            for (int st = 0; st < studentDataList.Count; st++)
            {
                var finalGrade = studentDataList[st].FinalGrade;
                var predictor = mlContext.Model.CreatePredictionEngine<StudentData, ScorePrediction>(model);
                var result = predictor.Predict(studentDataList[st]);

                //Распределение оценок
                switch (finalGrade)
                {
                    case 5:
                        class5_count++;
                        break;
                    case 4:
                        class4_count++;
                        break;
                    case 3:
                        class3_count++;
                        break;
                    case 2:
                        class2_count++;
                        break;
                }

                //Количество правильно угаданных оценок
                switch (result.PredictFG)
                {
                    case 5:
                        if (finalGrade == 5)
                            metrics[5]++;
                        break;
                    case 4:
                        if (finalGrade == 4)
                            metrics[4]++;
                        break;
                    case 3:
                        if (finalGrade == 3)
                            metrics[3]++;
                        break;
                    case 2:
                        if (finalGrade == 2)
                            metrics[2]++;
                        break;
                }
            }


            int[] ints = [
                class5_count, class4_count,
                class3_count, class2_count
            ];


            var index = 0;
            //Наглядное отображение
            if (getMatrix)
                foreach (var metric in metrics)
                    Console.WriteLine($"{metric.Key}: {metric.Value}, {ints[index++]}");
                Console.WriteLine();

            //Accuracy
            double Accuracy = 0;
            foreach (var value in metrics.Values)
                Accuracy += value;

            Accuracy = Math.Round(Accuracy / ints.Sum(), 4);
            return Accuracy;
        }
    }
}
