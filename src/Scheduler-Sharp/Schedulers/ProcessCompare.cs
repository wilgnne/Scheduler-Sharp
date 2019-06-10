using System;
using System.Collections.Generic;

using SchedulerSharp.Models;

namespace SchedulerSharp.Schedulers
{
    /// <summary>
    /// Comparador de processos
    /// </summary>
    public class ProcessCompare : IComparer<Process>
    {
        private enum Xy
        {
            X = -1,
            Both = 0,
            Y = 1
        };

        //the IComparer implementation wraps your readable code in an int cast.
        public int Compare(Process x, Process y)
        {
            return (int)CompareXy(x, y);
        }

        private static Xy CompareXy(Process x, Process y)
        {
            //Se os dois são nulos, retorna igual
            if (x == null && y == null) return Xy.Both;

            //Se um dos dois e nulo, retorna o outro
            if (x == null) return Xy.Y;
            if (y == null) return Xy.X;

            //Se o tempo de chegada e o runtime sao iguais
            if (x.arrivalTime == y.arrivalTime && x.runtime == y.runtime) return Xy.Both;

            //Se o tempo de chegada e igual, mas o runtime de X e menor
            if (x.arrivalTime == y.arrivalTime && x.runtime < y.runtime) return Xy.X;
            //Se o tempo de chegada e igual, mas o runtime de Y e menor
            if (x.arrivalTime == y.arrivalTime && x.runtime > y.runtime) return Xy.Y;

            if (x.arrivalTime < y.arrivalTime) return Xy.X;
            if (x.arrivalTime > y.arrivalTime) return Xy.Y;

            Console.WriteLine("Nao entrou em nada");
            return Xy.Both;
        }

    }
}
