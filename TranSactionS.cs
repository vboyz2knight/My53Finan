using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.CSharp.My53Finan
{

    public class TranSactionS : List<TranSaction>
    {
        private decimal tranTotalWithrawAmt = 0.00m;
        private decimal tranTotalDepositAmt = 00.00m;
        private decimal tranTotalBank = 00.00m;
        private decimal tranTotalInsurance = 00.00m;
        private decimal tranTotalCommunicationBill = 00.00m;
        private decimal tranTotalPetBill = 00.00m;
        private decimal tranTotalCable = 00.00m;
        private decimal tranTotalOther = 00.00m;
        private decimal tranTotalGrocery = 00.00m;
        private decimal tranTotalRestaurant = 00.00m;
        private decimal tranTotalCarBill = 00.00m;
        private decimal tranTotalHomeBill = 00.00m;


        public decimal mytranTotalBank
        {
            get
            {
                return tranTotalBank;
            }
        }

        public decimal mytranTotalWithrawAmt
        {
            get
            {
                return tranTotalWithrawAmt;
            }
        }

        public decimal mytranTotalDepositAmt
        {
            get
            {
                return tranTotalDepositAmt;
            }
        }

        public decimal mytranTotalInsurance
        {
            get
            {
                return tranTotalInsurance;
            }
        }

        public decimal mytranTotalCommunicationBill
        {
            get
            {
                return tranTotalCommunicationBill;
            }
        }

        public decimal mytranTotalPetBill
        {
            get
            {
                return tranTotalPetBill;
            }
        }

        public decimal mytranTotalCable
        {
            get
            {
                return tranTotalCable;
            }
        }

        public decimal mytranTotalOther
        {
            get
            {
                return tranTotalOther;
            }
        }

        public decimal mytranTotalGrocery
        {
            get
            {
                return tranTotalGrocery;
            }
        }

        public decimal mytranTotalRestaurant
        {
            get
            {
                return tranTotalRestaurant;
            }
            set
            {
                tranTotalRestaurant = value;
            }
        }

        public decimal mytranTotalCarBill
        {
            get
            {
                return tranTotalCarBill;
            }
        }

        public decimal mytranTotalHomeBill
        {
            get
            {
                return tranTotalHomeBill;
            }
        }

        public TranSactionS()
        {
            //
        }

        public void doStatistic()
        {
            if (this != null)
            {
                tranTotalWithrawAmt = 0.00m;
                tranTotalDepositAmt = 00.00m;
                tranTotalBank = 00.00m;
                tranTotalInsurance = 00.00m;
                tranTotalCommunicationBill = 00.00m;
                tranTotalPetBill = 00.00m;
                tranTotalCable = 00.00m;
                tranTotalOther = 00.00m;
                tranTotalGrocery = 00.00m;
                tranTotalRestaurant = 00.00m;
                tranTotalCarBill = 00.00m;
                tranTotalHomeBill = 00.00m;

                //Statistic calc
                foreach (TranSaction s in this)
                {
                    if (s.typeMy == "Withraw")
                    {
                        tranTotalWithrawAmt += s.Amt;
                    }
                    else if (s.typeMy == "Deposit")
                    {
                        tranTotalDepositAmt += s.Amt;
                    }
                    else
                    {
                        //MessageBox.Show("Error: Unknow Type data");
                    }

                    switch (s.myCategory)
                    {
                        case ("Insurance"):
                            tranTotalInsurance += Math.Abs(s.Amt);
                            break;
                        case ("HomeBill"):
                            tranTotalHomeBill += Math.Abs(s.Amt);
                            break;
                        case ("Restaurant"):
                            tranTotalRestaurant += Math.Abs(s.Amt);
                            break;
                        case ("Grocery"):
                            tranTotalGrocery += Math.Abs(s.Amt);
                            break;
                        case ("Other"):
                            tranTotalOther += Math.Abs(s.Amt);
                            break;
                        case ("Cable"):
                            tranTotalCable += Math.Abs(s.Amt);
                            break;
                        case ("CarBill"):
                            tranTotalCarBill += Math.Abs(s.Amt);
                            break;
                        case ("PetBill"):
                            tranTotalPetBill += Math.Abs(s.Amt);
                            break;
                        case ("CommunicationBill"):
                            tranTotalCommunicationBill += Math.Abs(s.Amt);
                            break;
                        case ("Bank"):
                            tranTotalBank += Math.Abs(s.Amt);
                            break;
                        default:
                            //lblError.Text +="Error unknow Category in data";
                            break;
                    }

                }
            }
            else
            {
                //MessageBox.Show("Error: Passing Null tranSactionS.");
            }
        }

    }
}