using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{


    class Program
    {
        enum CardSuit { HEART, DIAMOND, CLUB, SPADE };
        enum CardName { Jack = 2, Queen, King, Six = 6, Seven, Eight, Nine, Ten, Ace };
        //enum CardOwner { Computer, Player };

        struct Card
        {
            public CardSuit Suit;
            public CardName Name;
            public int Volume;
        }

        static Card[] PackOfCard()
        {

            Card[] result = new Card[36];

            int cardNumber = 0;

            foreach (var suit in Enum.GetValues(typeof(CardSuit)))
            {

                foreach (var cName in Enum.GetValues(typeof(CardName)))
                {
                    Card Card = new Card();
                    Card.Suit = (CardSuit)suit;
                    Card.Name = (CardName)cName;
                    Card.Volume = (int)cName;

                    result[cardNumber++] = Card;
                }

            }
            return result;
        }

        static void PackShuffle(Card[] pack)
        {
            Random rand = new Random();

            for (int i = 0; i < 1000; i++)
            {
                int cardNumber01 = rand.Next(0, 36);
                Card tempCard = new Card();
                tempCard = pack[cardNumber01];
                int cardNumber02 = rand.Next(0, 36);
                pack[cardNumber01] = pack[cardNumber02];
                pack[cardNumber02] = tempCard;
            }
        }

        static int InputChoiсe(string question)
        {
            int result = 0;
            bool inputRezult = false;
            while (inputRezult == false)
            {
                Console.Write(question);
                inputRezult = int.TryParse(Console.ReadLine(), out result);
                if (inputRezult == true)
                {
                    if (result != 1 && result != 2)
                    {
                        inputRezult = false;
                    }
                }
                if (inputRezult == false)
                {
                    Console.WriteLine("Don't correct input. Try again.");
                }
            }
            return result;
        }

        static int SummVolume(Card[] cards, out int qtyAce, bool showDetails = true)
        {
            int result = 0;
            qtyAce = 0;
            foreach (var card in cards)
            {
                if (card.Volume == 0)
                    break;
                result = result + card.Volume;
                if (card.Name == CardName.Ace)
                {
                    qtyAce++;
                }
                if (showDetails == true)
                {
                    Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
                }
                
            }
            return result;
        }

        static int[] Team(int whoFirstMove, int[] experienceComp)
        {
            Random rand = new Random();
            Card[] pack = PackOfCard();

            //foreach (var card in packOfCards)
            //{
            //    Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
            //}

            PackShuffle(pack);
            //Console.WriteLine("=============== After Shuffle ===== top 10 ===========");
            //int counter = 0;
            //foreach (var card in pack)
            //{
            //    if (++counter > 10) break;
            //    Console.WriteLine("{0} {1} - {2}", card.Name, card.Suit, card.Volume);
            //}
            string whoWin = "";
            int whoMove = whoFirstMove;

            Card[] playersCards = new Card[30];
            Card[] computersCards = new Card[30];

            bool finishMove = false;
            bool finishGame = false;

            int sumVolume = 0;
            int cardNumInPack = 0;
            int cardNumInPlayersPack = 0;
            int cardNumInComputersPack = 0;
            Card actCard = pack[cardNumInPack];  // выбираем из колоды первую карту
            while (finishGame != true)
            {
                if (whoMove == 1)   //  сейчас ходит Комп
                {
                    if (cardNumInPack < 4)   // первоначальная раздача по 2 карты
                    {
                        computersCards[cardNumInComputersPack++] = actCard;
                        actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                    }
                    else
                    {
                        finishMove = false;
                        while (finishMove == false)
                        {
                            //Console.WriteLine("===== Computers Cards");
                            bool showDetails = false;
                            sumVolume = SummVolume(computersCards, out int qtyAceComp, showDetails);
                            //Console.WriteLine("===== Computer has {0} points", sumVolume);
                            if (sumVolume == 21 || qtyAceComp == 2)
                            {
                                finishMove = true;
                                finishGame = true;
                                whoWin = "Computer";
                            }
                            else if (sumVolume > 21)
                            {
                                finishMove = true;
                                if (whoFirstMove == 2)
                                {
                                    finishGame = true;
                                }
                                else
                                {
                                    actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                }
                            }
                            else
                            {
                                //int result = InputChoiсe("Do You take next card(1) or finish(2) ? ");
                                int result = rand.Next(100);
                                //Console.WriteLine("rand = {0}, expeirence = {1}", result, experienceComp[sumVolume]);
                                if (result >= experienceComp[sumVolume])
                                {
                                    result = 2;
                                }
                                else
                                {
                                    result = 1;
                                    Console.WriteLine("   Computer takes one card else.");
                                }
                                if (result == 2)
                                {
                                    finishMove = true;
                                    if (whoFirstMove == 2)
                                    {
                                        finishGame = true;
                                    }
                                    else
                                    {
                                        actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                    }
                                }
                                else
                                {
                                    computersCards[cardNumInComputersPack++] = actCard;
                                    actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                }
                            }
                        }
                    }
                    whoMove = 2;
                }
                else   // сейчас ходит Player
                {
                    if (finishGame == true)
                    {
                        break;
                    }
                    if (cardNumInPack < 4)   // первоначальная раздача по 2 карты
                    {
                        playersCards[cardNumInPlayersPack++] = actCard;
                        actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                    }
                    else
                    {
                        finishMove = false;
                        while (finishMove == false)
                        {
                            Console.WriteLine("===== Players Cards");
                            sumVolume = SummVolume(playersCards, out int qtyAcePlay);
                            Console.WriteLine("===== You have {0} points", sumVolume);
                            if (sumVolume == 21 || qtyAcePlay == 2)
                            {
                                finishMove = true;
                                finishGame = true;
                                whoWin = "Player";
                            }
                            else if (sumVolume > 21)
                            {
                                finishMove = true;
                                if (whoFirstMove == 1)
                                {
                                    finishGame = true;
                                }
                                else
                                {
                                    actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                }
                            }
                            else
                            {
                                int result = InputChoiсe("Do You take next card(1) or finish(2) ? ");
                                if (result == 2)
                                {
                                    finishMove = true;
                                    if (whoFirstMove == 1)
                                    {
                                        finishGame = true;
                                    }
                                    else
                                    {
                                        actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                    }
                                }
                                else
                                {
                                    playersCards[cardNumInPlayersPack++] = actCard;
                                    actCard = pack[++cardNumInPack];  // выбираем из колоды следующую карту
                                }
                            }
                        }
                    }
                    whoMove = 1;
                };

            }
            Console.WriteLine();
            Console.WriteLine("RESULTS:");
            Console.WriteLine();
            Console.WriteLine("** Computer *************************************************************");
            int sumVolumeComp = SummVolume(computersCards, out int qtyAceCompF);
            Console.WriteLine("===== Computers total volume = {0}", sumVolumeComp);
            Console.WriteLine("** Player ***************************************************************");
            int sumVolumePlay = SummVolume(playersCards, out int qtyAcePlayF);
            Console.WriteLine("===== Players total volume = {0}", sumVolumePlay);
            Console.WriteLine("*************************************************************************");
            int[] resultArr = { 0, 0, 0};
            if (whoWin == "")
            {
                if (sumVolumeComp == sumVolumePlay)
                {
                    whoWin = "";
                }
                else if (sumVolumeComp < 21 && sumVolumePlay < 21)
                {
                    if (sumVolumeComp > sumVolumePlay)
                    {
                        whoWin = "Computer";
                    }
                    else
                    {
                        whoWin = "Player";
                    }
                }
                else if (sumVolumeComp > 21 && sumVolumePlay > 21)
                {
                    if (sumVolumeComp < sumVolumePlay)
                    {
                        whoWin = "Computer";
                    }
                    else
                    {
                        whoWin = "Player";
                    }
                }
                else if (sumVolumeComp < 21)
                {
                    whoWin = "Computer";
                }
                else
                {
                    whoWin = "Player";
                }
            }
            if (whoWin == "")
            {
                Console.WriteLine("!!!!!!!!!!!  =======  Nil - nil draw  =======  !!!!!!!!!!!");
                resultArr[2] = 1;
            }
            else if (whoWin == "Computer")
            {
                Console.WriteLine("!!!!!!!!!!!  =======  You Lose, Comp Win  =======  !!!!!!!!!!!");
                resultArr[0] = 1;
            }
            else
            {
                Console.WriteLine("!!!!!!!!!!!  =======  You WIN  =======  !!!!!!!!!!!");
                resultArr[1] = 1;
            }
            Console.WriteLine("*************************************************************************");

            return resultArr;
        }

        static int[] ComputerLearn()
        {
            Card[] pack;
            int[,] statistics = new int[21,2];
            for (int i = 0; i < 100000; i++)
            {
                pack = PackOfCard();
                PackShuffle(pack);
                int volume = 0;
                int newVolume = 0;
                for (int j = 0; j < 20; j++)
                {
                    if (volume > 20)
                    {
                        break;
                    }
                    newVolume = volume + pack[j].Volume;
                    if (newVolume < 22)
                    {
                        statistics[volume, 0] = statistics[volume, 0] + 1;
                    }
                    else
                    {
                        statistics[volume, 1] = statistics[volume, 1] + 1;
                    }
                    volume = newVolume;
                }
            }
            //for (int i = 0; i < statistics.GetLength(0); i++)
            //{
            //    Console.WriteLine("When I have {0} point and resived card, I was successful {1} and miss {2} teams", i, statistics[i, 0], statistics[i, 1]);
            //}
            int[] resultStat = new int[21];
            for (int i = 0; i < resultStat.Length; i++)
            {
                if (statistics[i, 1] == 0)
                {
                    resultStat[i] = 100;
                }
                else
                {
                    resultStat[i] = 100 * statistics[i, 0] / (statistics[i, 1] + statistics[i, 0]);
                }
                //Console.WriteLine(" i = {0}, successPercent = {1}", i, resultStat[i]);    
            }
            //Console.ReadKey();
            return resultStat;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please wait couple of seconds. Computer is learning this Game.");
            int[] experienceComp = ComputerLearn();
            Console.Clear();

            int totalWinComp = 0;
            int totalWinPlay = 0;
            int totalNil = 0;
            int resultQuestion = 1;
            int[] resTeam = { 0, 0, 0};
            int whoFirstMove = InputChoiсe("Who make first Move ? (Computer - 1, Player - 2) ");
            do
            {
                Console.Clear();
                Console.WriteLine("*******New Team**********************************************************");
                Console.WriteLine();
                Console.WriteLine("{0} doing first Move.", (whoFirstMove == 1) ? "Computer is " : "You are ");
                resTeam = Team(whoFirstMove, experienceComp);
                totalWinComp = totalWinComp + resTeam[0];
                totalWinPlay = totalWinPlay + resTeam[1];
                totalNil = totalNil + resTeam[2];
                Console.WriteLine();
                Console.WriteLine("   TOTAL RESULTS:");
                Console.WriteLine();
                Console.WriteLine("** You Won {0} teams, Computer Won {1} teams and was {2} nil-nil draw",totalWinPlay,totalWinComp, totalNil);
                Console.WriteLine();
                resultQuestion = InputChoiсe("Do You want continue(1) or finish(2) ? ");
                whoFirstMove = (whoFirstMove == 1) ? 2 : 1;
            }
            while (resultQuestion == 1);
           
        }
    }
}
