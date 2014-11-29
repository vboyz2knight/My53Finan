using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.CSharp.My53Finan
{
    public class MyGraph: List <MyGraphData>
    {
        public List<MyGraphData> GraphDatas = null;

        public MyGraph(TranSactionS myDrawTransactions)
        {
            buildMyGraphDatas(myDrawTransactions);
        }

        public MyGraph(TranSactionS myDrawTransactions, string desiredCategory )
        {
            buildMyGraphDatas(myDrawTransactions, desiredCategory);
        }

        private bool buildMyGraphDatas(TranSactionS myDrawTransactions, string desiredCategory)
        {
            bool bReturn = false;

            if (myDrawTransactions != null)
            {
                if (GraphDatas != null)
                {
                    GraphDatas.Clear();
                }
                else
                {
                    GraphDatas = new List<MyGraphData>();
                    foreach (TranSaction s in myDrawTransactions)
                    {
                        GraphDatas.Add(new MyGraphData { Category = s.myFilter, Amount = Math.Abs(s.Amt) });
                    }

                }

                bReturn = true;
            }

            return bReturn;
        }

        public bool buildMyGraphDatas(TranSactionS myDrawTransactions)
        {
            bool bReturn = false;

            if (myDrawTransactions != null)
            {
                if (GraphDatas != null)
                {
                    GraphDatas.Clear();
                }
                else
                {
                    GraphDatas = new List<MyGraphData>();
                }

                myDrawTransactions.doStatistic();

                if (Math.Abs(myDrawTransactions.mytranTotalBank) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalBank", Amount = Math.Abs(myDrawTransactions.mytranTotalBank) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalInsurance) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalInsurance", Amount = Math.Abs(myDrawTransactions.mytranTotalInsurance) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalCommunicationBill) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalCommunicationBill", Amount = Math.Abs(myDrawTransactions.mytranTotalCommunicationBill) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalPetBill) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalPetBill", Amount = Math.Abs(myDrawTransactions.mytranTotalPetBill) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalCable) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalCable", Amount = Math.Abs(myDrawTransactions.mytranTotalCable) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalOther) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalOther", Amount = Math.Abs(myDrawTransactions.mytranTotalOther) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalGrocery) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalGrocery", Amount = Math.Abs(myDrawTransactions.mytranTotalGrocery) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalRestaurant) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalRestaurant", Amount = Math.Abs(myDrawTransactions.mytranTotalRestaurant) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalCarBill) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalCarBill", Amount = Math.Abs(myDrawTransactions.mytranTotalCarBill) });
                }

                if (Math.Abs(myDrawTransactions.mytranTotalHomeBill) > 0)
                {
                    GraphDatas.Add(new MyGraphData { Category = "TotalHomeBill", Amount = Math.Abs(myDrawTransactions.mytranTotalHomeBill) });
                }

                bReturn = true;
            }

            return bReturn;
        }
    }
}