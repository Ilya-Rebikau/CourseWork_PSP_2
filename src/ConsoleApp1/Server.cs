namespace ConsoleApp1
{
    internal class Server
    {
        public float[] strokaToSend;
        public Dictionary<int, float[]> stroku;
        public float precision = 0.001F;
        public float[] strokaToGet;
        public float GetX(float vectorXNumber, float matrixNumber)
        {
            return vectorXNumber / matrixNumber;
        }

        public float GetX(float vectorXNumber, float currentX, float previousMatrixNumber)
        {
            return vectorXNumber - previousMatrixNumber * currentX;
        }

        public void Minus(int startIndex)
        {
            for (int i = 0; i < stroku.Values.Count; i++)
            {
                if (stroku.Keys.ElementAt(i) > startIndex)
                {
                    var indexOfFirstNotZero = FirstNotZero(stroku.Values.ElementAt(i));
                    var valueOfFirstNotZero = stroku.Values.ElementAt(i)[indexOfFirstNotZero];
                    var secondValue = strokaToGet[indexOfFirstNotZero];
                    if (!CompareStroku(stroku.Values.ElementAt(i), strokaToSend))
                    {
                        for (int j = 0; j < stroku.Values.ElementAt(i).Length; j++)
                        {
                            var currentValue = stroku.Values.ElementAt(i)[j];
                            var removeValue = strokaToGet[j] * (valueOfFirstNotZero / secondValue);
                            if (currentValue - removeValue > 0 &&
                                currentValue - removeValue < precision)
                            {
                                stroku.Values.ElementAt(i)[j] = 0;
                            }
                            else
                            {
                                stroku.Values.ElementAt(i)[j] -= removeValue;
                            }
                        }
                    }
                }
            }
        }

        private int FirstNotZero(float[] stroka)
        {
            var notZero = stroka.First(x => x != 0);
            return stroka.ToList().IndexOf(notZero);
        }

        private bool CompareStroku(float[] stroka1, float[] stroka2)
        {
            try
            {
                for (int i = 0; i < stroka1.Length; i++)
                {
                    if (stroka1[i] != stroka2[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
