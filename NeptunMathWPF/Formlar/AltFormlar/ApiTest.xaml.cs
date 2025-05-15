using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using NeptunMathWPF.Models;
using System.Configuration;
using NeptunMathWPF.SoruVeAjani.Problemler;

namespace NeptunMathWPF.Formlar
{
    public partial class ApiTest : Window
    {

        public ApiTest()
        {
            InitializeComponent();
            BtnCoz.IsEnabled = false;
        }

        private async void BtnUret_Click(object sender, RoutedEventArgs e)
        {
            var repo = await ProblemGenerator.GenerateProblem(ProblemType.Hareket, ProblemDifficulty.CokZor);

                string problem = $"{repo.problem}\nDoğru: {repo.correctAnswer}\nYanlışlar:";

            
            foreach (var item in repo.incorrectAnswers)
                {
                    problem += item.ToString() + "\n";
                }

                TxtProblem.Text = problem;
            

        }

        private async void BtnCoz_Click(object sender, RoutedEventArgs e)
        {
            TxtCozum.Text = "";

        }


    }
}
