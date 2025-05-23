﻿using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.SoruVeAjani
{
    class PDFLatexYineleyici
    {


        /// <summary>
        /// Soru Modellerini Batch ile pdflatex komut istemcisi oluşturur, 
        /// bu istemciyle soruların LaTeX pdf çıktısı oluşturur
        /// </summary>
        /// <param name="Modeller"></param>
        public void LaTeXPDFolustur(Collection<SoruCardModel> Modeller, string KayitYeri)
        {
            Genel.Handle(() =>
            {

                string[] latexContent = LatexArgumanString(Modeller);
                string ciktiDIR = KayitYeri;

                if (!Directory.Exists(ciktiDIR))
                {
                    Directory.CreateDirectory(ciktiDIR);
                }

                string tempTexFile = "belge.tex";

                File.WriteAllLines(tempTexFile, latexContent);

                string arguments = $"\"{tempTexFile}\" \"{ciktiDIR}\"";

                Process _Surec = new Process();
                _Surec.StartInfo.FileName = "C:\\Users\\Bilgisayar\\Source\\Repos\\DocumentDeneme\\LimitQuizApp\\pdflatexBatch.bat";
                _Surec.StartInfo.Arguments = arguments;
                _Surec.StartInfo.UseShellExecute = false;
                _Surec.Start();


                _Surec.WaitForExit();
            });
        }


        /// <summary>
        /// CMD pdflatex için komutlar argüman listesi geri gönderir.
        /// gövdesindeki argümanlar oluşturulan belgenin şablonudur
        /// </summary>
        /// <returns></returns>
        public string[] LatexArgumanString(Collection<SoruCardModel> Modeller)
        {

            List<char> cevapkagidi = new List<char>();
            List<string> latexContextListesi = new List<string>()
            {
                "\\documentclass{article}",
                "\\usepackage{amsmath}",
                "\\usepackage{amsfonts}",
                "\\usepackage{amssymb}",

                "\\makeatletter",
                "\\def\\UTFviii@undefined@err#1{?gçrs?}",
                "\\makeatother",

                "\\begin{document}",

                "\\fontsize{15pt}{20pt}\\selectfont",

                "\\title{NeptunWPF soru cıktısı}",
                "\\section{Sorular}"
            };

            int i = 0;
            foreach (SoruCardModel _model in Modeller)
            {
                i++;
                latexContextListesi.Add($"\\subsection*{{Soru {i}}}");

                if (_model.Tur == SoruTerimleri.soruTuru.islem)
                {
                    latexContextListesi.Add($"${_model.LaTeX}$");
                }
                else
                {
                    if (_model.Tur == SoruTerimleri.soruTuru.fonksiyon)
                    {

                        string ae = $"$\\text{{{_model.soru.IslemMetin}}}$";

                        ae = ae.Replace("⁻", "\\mp");
                        ae = ae.Replace("^", "");

                        latexContextListesi.Add(ae);
                        
                    }
                    else
                    {
                        latexContextListesi.Add($"{_model.LaTeX}");
                    }
                }

                latexContextListesi.AddRange(new string[] { "\\\\" });
                latexContextListesi.AddRange(new string[] { "\\," });


                char opt = 'A';
                foreach (string secenek in _model.NesneSecenekler.secenekler)
                {
                    string n = secenek.Replace("ℝ", "R");
                    latexContextListesi.Add($"{opt} : {n}");
                    latexContextListesi.Add("\\,");

                    if (secenek == _model.NesneSecenekler.DogruSecenekGetir())
                        cevapkagidi.Add(opt);

                    opt++;
                }

                latexContextListesi.AddRange(new string[] { "\\, \\," });
            }

            latexContextListesi.Add("\\section*{CEVAPLAR}");

            latexContextListesi.AddRange(new string[] { "\\," });

            int c = 0;
            foreach (char karakter in cevapkagidi)
            {
                c++;

                latexContextListesi.Add($"{c} :: {karakter}");
                latexContextListesi.Add("\\hspace{1pt}");
            }

            latexContextListesi.Add("\\end{document}");

            return latexContextListesi.ToArray();
        }
    }
}
