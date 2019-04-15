using System;
using System.Collections.Generic;

namespace SchedulerSharp.Schedulers
{
    public class Scheduler
    {
        public List<LogProcess> Finish { get; protected set; }
        protected List<Process> forExec;

        protected int clkTime = 0;

        public virtual void ProcessData() { }

        /*Executar Processo*/
        protected void RunProcess (Process process)
        {
            /*Atualiza o tempo de execução do processo*/
            process.Run();
            /*Adiciona uma referencia do mesmo a lista de finalizados*/
            Finish.Add(new LogProcess (process, clkTime));
            /*Se o processo tiver terminado sua execução por completo o remove
            da lista de processos para execução*/
            if (process.Runtime == 0)
            {
                forExec.Remove(process);
            }
            /*Passa para o proximo tempo de clock*/
            clkTime += 1;
        }
    }
}
