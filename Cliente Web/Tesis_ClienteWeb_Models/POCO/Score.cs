using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Score
    {
        //Required - FluentAPI
        public int AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

        //Required - FluentAPI
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public float NumberScore { get; set; }

        public string LetterScore { get; set; }

        /// <summary>
        /// Método para operar sobre los literales. Se utilizan valores de referencias en números, siendo el
        /// mayor valor A = 5, y el menor valor E = 1. Si devuelve 0 es porque se pasó un literal no válido.
        /// Rodrigo Uzcátegui - 30-04-15
        /// </summary>
        /// <param name="Letter">El literal que representa la nota</param>
        /// <returns>Su valor referencial en número</returns>
        public int ToIntLetterScore(string Letter)
        {
            if (Letter.ToUpper().Equals("A"))
                return 5;
            else if (Letter.ToUpper().Equals("B"))
                return 4;
            else if (Letter.ToUpper().Equals("C"))
                return 3;
            else if (Letter.ToUpper().Equals("D"))
                return 2;
            else if (Letter.ToUpper().Equals("E"))
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Método que devuelve el valor asociado al literal. Se utilizan valores de referencias en números, 
        /// siendo el mayor valor A = 5, y el menor valor E = 1. Si devuelve "" es porque se pasó un número no 
        /// válido.
        /// Rodrigo Uzcátegui - 30-04-15
        /// <param name="intLetter">El valor numérico asociado al literal</param>
        /// <returns>El valor literal respectivo.</returns>
        public string ToStringLetterIntScore(int intLetter)
        {
            if (intLetter == 5)
                return "A";
            else if (intLetter == 4)
                return "B";
            else if (intLetter == 3)
                return "C";
            else if (intLetter == 2)
                return "D";
            else if (intLetter == 1)
                return "E";
            else
                return "";
        }
    }
}
