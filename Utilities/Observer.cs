using System;
using System.Collections.Generic;
using System.Linq;

namespace UMDGeneral.Utilities
{
    public class AnonDisposable : IDisposable
    { //if only AnonymousDisposable was public this would not be needed
        Action action;

        public AnonDisposable(Action a)
        {
            action = a;
        }

        public void Dispose()
        {
            action();
        }
    }
    public interface IObservable2<T>// : IObservable<T>
    {
        IDisposable Subscribe(Action<T> lambda);
    }
    public class ObservableEvent<T> : IObservable2<T>
    {
        List<Action<T>> currentObservers = new List<Action<T>>();
        
        public void Raise(T args)
        {
            foreach (var it in currentObservers)
                //foreach(var it in ob)
                    it(args);
            //foreach (var observer in currentObservers)
            //    observer.OnNext(value);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            //List<Action<T>> oAct;
            //if (!currentObservers.TryGetValue(observer, out oAct))
            //currentObservers.Add(observer, new List<Action<T>>() { action });
            //else
            currentObservers.Add(action);
           // Subscribe(observer);
            return new AnonDisposable(() => currentObservers.Remove(action));
        }
        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Action<IObserver<T>> lambda)
        {
            //Subscribe()
            foreach (var lm in lambda.GetInvocationList())
            {
                if (lm.GetType().ToString().Contains("ResponseMessage"))
                {
                 //   Subscribe()
                }
                Console.WriteLine("Meth = " + lm.Method + "  Target : " + lm.Target);
                Console.WriteLine("Meth params = " + lm.Method.GetParameters());
            };
             //== typeof(Websocket.Client.ReconnectionType)

            
            //Subscribe()
            return new AnonDisposable(() => Console.Write("lambda" + lambda.Target.ToString()));
        }


        //public IDisposable Subscribe(IObserver<T> observer)
        //{
        //    throw new NotImplementedException();
        //}
        //        public IDisposable Subscribe(Action<IObserver<T>> lambda, IObserver<T> obs)
        //        {

        //            foreach (var lm in lambda.GetInvocationList())
        //            {
        //                Console.WriteLine("Meth = " + lm.Method + "  Target : " + lm.Target);
        //                Console.WriteLine("Meth params = " + lm.Method.GetParameters());
        //            };

        ////            currentObservers1.Add(lambda.);
        //            return new AnonDisposable(() => currentObservers.Remove(obs));
        //        }


        //public IDisposable Subscribe(IObserver<T> observer)
        //{
        //    throw new NotImplementedException();
        //}

        //public IDisposable Subscribe(IObserver<T> type, Action<IObserver<T>> lambda)
        //{
        //    Type typeParameterType = typeof(T);

        //  //  currentObservers.Add(type, lambda);
        //    return new AnonDisposable(() => currentObservers.Remove(observer));

        //    //throw new NotImplementedException();
        //}
    }
}
