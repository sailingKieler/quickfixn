using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using QuickFix.Fields;
using QuickFix.Logger;
using QuickFix.Store;

namespace TradeClient
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("=============");
            Console.WriteLine("This is only an example program, meant to run against the Executor or SimpleAcceptor example programs.");
            Console.WriteLine();
            Console.WriteLine("                                                    ! ! !");
            Console.WriteLine("              DO NOT USE THIS ON A COMMERCIAL FIX INTERFACE!  It won't work and it's a bad idea!");
            Console.WriteLine("                                                    ! ! !");
            Console.WriteLine();
            Console.WriteLine("=============");

            if (args.Length != 1)
            {
                System.Console.WriteLine("usage: TradeClient.exe CONFIG_FILENAME");
                System.Environment.Exit(2);
            }

            string file = args[0];

            try
            {
                QuickFix.SessionSettings settings = new QuickFix.SessionSettings(file);
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new ScreenLogFactory(settings);
                TradeClientApp application = new TradeClientApp();
                QuickFix.Transport.SocketInitiator initiator = new QuickFix.Transport.SocketInitiator(application, storeFactory, settings, logFactory);

                // this is a developer-test kludge.  do not emulate.
                application.MyInitiator = initiator;

                initiator.Start();

                // new Thread(() => {
                //     Thread.Sleep(5000);
                //     QuickFix.FIX44.NewOrderSingle newOrderSingle = new QuickFix.FIX44.NewOrderSingle(
                //         new ClOrdID("Foo"),
                //         new Symbol("Bar"),
                //         new Side(Side.BUY),
                //         new TransactTime(DateTime.Now),
                //         new OrdType(OrdType.MARKET)
                //     );

                //     newOrderSingle.SetFields([
                //         new OrderQty(1),
                //         new HandlInst(HandlInst.AUTOMATED_EXECUTION_ORDER_PRIVATE),
                //         new TimeInForce(TimeInForce.DAY)
                //     ]);

                //     application.SendMessage(newOrderSingle);

                // }).Start();
                
                application.Run();
                initiator.Stop();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            Environment.Exit(1);
        }
    }
}
