//using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carshops_MVC
{
    internal class OrderProcessor
    {
        readonly uint transactionCount;
        readonly Queue<SellCommand> sellOperations;
        readonly Queue<RefaundCommand> refaundOperations;
        //private static Logger logger;

        private static OrderProcessor instance;

        protected OrderProcessor(uint transactionCount)
        {
            sellOperations = new Queue<SellCommand>();
            refaundOperations = new Queue<RefaundCommand>();    
            this.transactionCount = transactionCount;
            //logger = LogManager.GetCurrentClassLogger();
        }

        private void Rollback<TCommand>(Queue<TCommand> executedOperations) where TCommand : ISellCommand
        {
            //logger.Error("Ошибка транзакции. Отмена выполненных операций.");
            while (executedOperations.Count != 0)
            {
                executedOperations.Dequeue().Undo();
            }
            //logger.Info("Отмена прошла успешно.");
        }

        private void Transact<TCommand>(Queue<TCommand> executedOperations, string status, string brand, string model) where TCommand : ISellCommand
        {
            var rollbackOperations = new Queue<TCommand>();
            TCommand reverseCommand;
            uint count = 0;
            //int i = 0;
            try
            {
                //logger.Info("Запущено выполнение транзакции.");
                while (executedOperations.Count != 0)
                {
                    reverseCommand = executedOperations.Dequeue();
                    rollbackOperations.Enqueue(reverseCommand);
                    reverseCommand.Execute();
                    //if (i == 2)
                    //    reverseCommand.Cancellation();
                    //i++;
                    count++;
                }
                //logger.Info("Транзакция прошла успешно.");
                //logger.Info($"{status} {count} автомобилей марки {brand} модели {model}");
            }
            catch 
            {
                Rollback(rollbackOperations);
            }
        }

        public void BuyCars(Office office, string model, uint quantity)
        {
            sellOperations.Enqueue(new SellCommand(office, model, quantity));
            if (sellOperations.Count == transactionCount)
            {
                Transact(sellOperations, "Продано", office.Brand, model);
            }
        }
        public void RefaundCars(Office office, string model, uint quantity)
        {
            refaundOperations.Enqueue(new RefaundCommand(office, model, quantity));
            if (refaundOperations.Count == transactionCount)
            {
                Transact(refaundOperations, "Возвращено", office.Brand, model);
            }
        }

        public static OrderProcessor GetInstance(uint transactionCount)
        {
            if (instance == null)
                instance = new OrderProcessor(transactionCount);
            return instance;
        }
    }
}
