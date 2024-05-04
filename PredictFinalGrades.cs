using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PredictFinalGrades
{
    class StudentData
    {
        public string StudentId { get; set; }

        public string Professor { get; set; }

        public float ScoresSum { get; set; } // Массив для хранения баллов по всем темам

        [ColumnName("Label")]
        public float FinalGrade { get; set; } // Массив для хранения баллов по всем темам

    }

    public class ScorePrediction
    {
        [ColumnName("Score")]
        public float FinalGrade { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            var mlContext = new MLContext();

            var allLines = File.ReadAllLines("C:/Users/One/Desktop/dataset2.csv");
            int length = allLines.Length;
            int index = 1;

            var studentDataList = new List<StudentData>(); //Список студентов

            string controleString = "Основные принципы организации Языка Python. Базовые элементы программирования и типы данных0";

            while (index < length)
            {

                var theme = allLines[index].Split(";")[3]; //Тема
                var obj = allLines[index].Split(";")[2].Split(" ")[1][0]; // П или Л (практика или лекция)

                if (theme == controleString & obj == 'П')
                {
                    var len = index;
                    var studentId = allLines[index].Split(';')[0]; //ID студента
                    var professor = allLines[index].Split(';')[10]; //Преподаватель
                    var finalGrade = float.Parse(allLines[index].Split(';')[7]); //Итоговый балл

                    /*Console.WriteLine($"i = {index}");*/

                    var scoresSum = 0f;
                    //Подсчет суммы накопленных баллов за:
                    // 9: 1 контрольная + 8 лаб
                    // 13: 2 контрольные + 11 лаб
                    var n = 9;
                    for (int i = len; i < len + n; i++)
                    {
                        index++;

                        var score = 0f;
                        var line = allLines[i].Split(';');
                        if (line[4] != "")
                            score = float.Parse(line[4]);

                        if (line[6] != "")
                            score += float.Parse(line[6]);

                        scoresSum += score;
                    }
                    /*Console.WriteLine($"scoresSum = {scoresSum}");
                    Console.WriteLine($"finalGrade = {finalGrade}");

                    Console.WriteLine("-----------");*/

                    studentDataList.Add(new StudentData
                    {
                        StudentId = studentId,
                        Professor = professor,
                        ScoresSum = scoresSum,
                        FinalGrade = finalGrade
                    });
                }
                index += 1;
            }

            // Преобразование списка в IDataView
            var dataView = mlContext.Data.LoadFromEnumerable(studentDataList);

            // Разделение данных на обучающую и тестовую выборки
            var splitData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2, seed:0);
            var trainData = splitData.TrainSet;
            var testData = splitData.TestSet;

            // Определение признаков и метки для обучения
            var pipeline = mlContext.Transforms
                .Categorical.OneHotEncoding(inputColumnName: "Professor", outputColumnName: "Professor")
                .Append(mlContext.Transforms.Concatenate("Features", nameof(StudentData.Professor), nameof(StudentData.ScoresSum)))
                .Append(mlContext.Regression.Trainers.Sdca());

            // Обучение модели
            var model = pipeline.Fit(trainData);

            // Оценка модели
            var predictions = model.Transform(testData);
            var metrics = mlContext.Regression.Evaluate(predictions);

            // Вывод метрик
            Console.WriteLine($"RMSE: {metrics.RootMeanSquaredError}");
            Console.WriteLine($"R^2: {metrics.RSquared:0.##}");

            /* var scores = mlContext.Regression.CrossValidate(dataView, pipeline, numberOfFolds: 5);
             var mean = scores.Average(x => x.Metrics.RSquared);
             Console.WriteLine($"Mean cross-validated R2 score: {mean:0.##}");*/

            // Прогнозирование
            Console.WriteLine("Блок прогнозирования");

            var randomSTList = studentDataList.Slice(300, 100);

            for (int st = 0; st < randomSTList.Count; st++)
            {
                var scoreRS = randomSTList[st].ScoresSum;
                var professorRS = randomSTList[st].Professor;
                var finalGradeRS = randomSTList[st].FinalGrade;

                var predictor = mlContext.Model.CreatePredictionEngine<StudentData, ScorePrediction>(model);
                var newStudentData = new StudentData { ScoresSum = scoreRS, Professor = professorRS };
                var result = predictor.Predict(newStudentData);

                Console.WriteLine($"Преподаватель: {professorRS}");
                Console.WriteLine($"Сумма баллов: {newStudentData.ScoresSum}");
                Console.WriteLine($"Реальный итоговый балл: {finalGradeRS}");
                Console.WriteLine($"Прогнозируемый итоговый балл: {result.FinalGrade}");

                Console.WriteLine(new String('-', 40));
            }
        }
    }
}
