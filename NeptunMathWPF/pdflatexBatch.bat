@echo off
setlocal

set "latex_file=%1"
set "output_dir=%2"

echo pdflatex "%latex_file%" de calistiriliyor with hedef directory "%output_dir%"...
pdflatex -shell-escape -output-directory="%output_dir%" "%latex_file%"

if errorlevel 1 (
  echo LaTeX derlemesinde hata.
) else (
  echo LaTeX derlemesi basarili. PDF "%output_dir%" kaydedildi.
)

endlocal

