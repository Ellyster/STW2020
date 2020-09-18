using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

using MovieWars.DTOs;

namespace MovieWars
{
    class Program
    {
        static void Main(string[] args)
        {
            string trainingFilename = "ratings_training.csv";
            string testFilename = "ratings_test.csv";
            MovieRating example = new MovieRating { userId = 1, movieId = 441 };
            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options
              {
                  MatrixColumnIndexColumnName = "userIdEncoded",
                  MatrixRowIndexColumnName = "movieIdEncoded", 
                  LabelColumnName = "Label",
                  NumberOfIterations = 10,
                  ApproximationRank = 25,
              };
            

            MLContext mlContext = new MLContext();
            (IDataView trainingData, IDataView testData) = LoadData(mlContext, trainingFilename, testFilename);          

            IEstimator<ITransformer> pipeline = BuildMatrixFactorizationPipeline(mlContext, options);
            ITransformer model = TrainModel(mlContext, pipeline, trainingData);

            var trainingMetrics = EvaluateModel(mlContext, model, "TRAINING", trainingData);
            DisplayMetrics("TRAINING", trainingMetrics);

            var testMetrics = EvaluateModel(mlContext, model, "TEST", testData);
            DisplayMetrics("TEST", testMetrics);

            var prediction = PredictWithModel(mlContext, model, example);
            DisplayPrediction(example, prediction);

            //SaveModel(mlContext, model, options, testMetrics.RSquared, trainingData.Schema);
        }

        public static (IDataView training, IDataView test) LoadData(MLContext mlContext, string trainingFilename, string testFilename)
        {
          Console.WriteLine("=============== Loading datasets ===============" + "\n");

          var trainingDatasetPath = Path.Combine(Environment.CurrentDirectory, "data", trainingFilename);
          var testDatasetPath = Path.Combine(Environment.CurrentDirectory, "data", testFilename);
          
          IDataView trainingData = mlContext.Data.LoadFromTextFile<MovieRating>(trainingDatasetPath, hasHeader: true, separatorChar: ',');
          IDataView testData = mlContext.Data.LoadFromTextFile<MovieRating>(testDatasetPath, hasHeader: true, separatorChar: ',');

          return (trainingData, testData);
        }

        public static IEstimator<ITransformer> BuildMatrixFactorizationPipeline(MLContext mlContext, MatrixFactorizationTrainer.Options options)
        {
          Console.WriteLine("=============== Building the model ===============" + "\n");
          
          // TASK 1: Create a funtion that returns training pipeline for the Matrix Factorization algorithm with the proper options.

          return null;
        }

        public static ITransformer TrainModel(MLContext mlContext, IEstimator<ITransformer> pipeline, IDataView trainingData)
        {
          Console.WriteLine("=============== Training the model ===============");

          // TASK 2: Create a function that trains and returns the model with the training pipeline and the training dataset.

          return null;
        }

        public static RegressionMetrics EvaluateModel(MLContext mlContext, ITransformer model, string datasetName, IDataView data)
        {
          Console.WriteLine("=============== Evaluating the model (" + datasetName + ") ===============");
         
          // TASK 3: Create a function that evaluates a ginven model with the provided dataset and returns its metrics.

          return null;
        }

        public static MovieRatingPrediction PredictWithModel(MLContext mlContext, ITransformer model, MovieRating example)
        {
          Console.WriteLine("=============== Making a prediction ===============");

          // TASK 4: Create a function that makes and returns a prediction for an example with the given model.

          return null;
        }

        public static void SaveModel(MLContext mlContext, ITransformer model, MatrixFactorizationTrainer.Options options, double rsquared, DataViewSchema dataSchema)
        {
            Console.WriteLine("=============== Saving the model ===============");
            
            var tags = new string[] {
              DateTime.Now.Ticks.ToString(), 
              "MovieWars", 
              "MatrixFactorization", 
              Math.Round(rsquared * 100).ToString(), 
              options.NumberOfIterations.ToString(), 
              options.ApproximationRank.ToString()
            };

            var modelFilename = string.Join("_", tags) + ".zip";
            var modelPath = Path.Combine(Environment.CurrentDirectory, "models", modelFilename);

            mlContext.Model.Save(model, dataSchema, modelPath);

            Console.WriteLine("Your model was saved as " + modelFilename + "\n");
        }

        public static void DisplayMetrics(string datasetName, RegressionMetrics metrics)
        {
          if(metrics != null){
            Console.WriteLine("Metrics from " + datasetName + " data");
            Console.WriteLine("Mean Absolute Error : " + metrics.MeanAbsoluteError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString() + "\n");
          } else {
            Console.WriteLine("Task 3 \"Evaluate model\" not completed.");
          }   
        }

        public static void DisplayPrediction(MovieRating example, MovieRatingPrediction prediction)
        {
          if(prediction != null)
            Console.WriteLine("The predicted score of user " + example.userId + " for the movie " + example.movieId + " is: " + Math.Round(prediction.Score, 1) + "\n");
          else
            Console.WriteLine("Task 4 \"Predict\" not completed.");
        }
    }
}
