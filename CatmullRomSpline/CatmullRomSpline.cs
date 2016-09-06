using System;
using System.Collections.Generic;

namespace CatmullRomSplines {
    public class CatmullRomSpline {

        /// <summary>
        /// Вычисляет узловые CatmullRom-сплайна.
        /// https://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
        /// </summary>
        /// <param name="aPoints">Полюса сплайна (исходные точки). Должно быть не менее 2-х полюсов.</param>
        /// <param name="aTension">Натяжение сплайна.</param>
        /// <param name="n">Число узлов между полюсами сплайна.</param>
        /// <param name="aIsClosedCurve">True - сплайн будет замкнут, false - иначе.</param>
        /// <returns></returns>
        public static Vector2[] Calculate(Vector2[] aPoints, double aTension, int n = 5, bool aIsClosedCurve = true) {
            if (aPoints == null) {
                throw new ArgumentNullException("aPoints");
            }

            if (aPoints.Length <= 2) {
                throw new ArgumentException("Число полюсов должно быть > 2.");
            }            

            if (n < 1) {
                throw new ArgumentException("Число узлов между полюсами сплайна должно быть >= 1.");
            }

            var N = aPoints.Length;

            var vectors = BuildVectors(aPoints, aIsClosedCurve);
            var resultSpline = new List<Vector2>();

            for (var i = 1; i < vectors.Length - 2; ++i) {
                var singleSpline = CalculateSpline(aTension, n, vectors[i - 1], vectors[i], vectors[i + 1], vectors[i + 2]);
                resultSpline.AddRange(singleSpline);
            }

            return resultSpline.ToArray();            
        }

        /// <summary>
        /// Строит вектора, по которым будут расчитываться сплайны.
        /// </summary>
        /// <param name="aPoints">Исходные точки.</param>
        /// <param name="aIsClosedCurve">Индикатор замкнутости.</param>
        /// <returns>Вектора.</returns>
        private static Vector2[] BuildVectors(Vector2[] aPoints, bool aIsClosedCurve) {
            var N = aPoints.Length;
            Vector2[] vectors;
            if (aIsClosedCurve) {
                vectors = new Vector2[N + 3];
                vectors[0] = new Vector2(aPoints[N - 1]);
                vectors[N + 1] = new Vector2(aPoints[0]);
                vectors[N + 2] = new Vector2(aPoints[1]);
            } else {
                vectors = new Vector2[N + 2];
                vectors[0] = new Vector2(aPoints[0]);
                vectors[N + 1] = new Vector2(aPoints[N - 1]);
            }
            for (var i = 0; i < N; ++i) {
                vectors[i + 1] = new Vector2(aPoints[i]);
            }

            return vectors;
        }

        /// <summary>
        /// Расчитывает вектора для одного участка сплайна, ограниченного p0, p1, p2 и p3.
        /// </summary>
        /// <param name="aTension">Натяжение.</param>
        /// <param name="n">Количество узлов между полюсами.</param>
        /// <param name="p0">Полюс 1.</param>
        /// <param name="p1">Полюс 2.</param>
        /// <param name="p2">Полюс 3.</param>
        /// <param name="p3">Полюс 4.</param>
        /// <returns>Вектора сплайна.</returns>
        private static Vector2[] CalculateSpline(double aTension, int n, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
            var resultVectors = new Vector2[n + 1];
            var vectorIndex = 0;
            var step = 1.0 / n;
            for (var t = 0; t < n; ++t) {
                var vector = ReturnCarmullRomVector(t * step, aTension, p0, p1, p2, p3);
                resultVectors[vectorIndex++] = vector;
            }
            resultVectors[vectorIndex] = new Vector2(p2);

            return resultVectors;
        }

        /// <summary>
        /// Расчитывает конкретный вектор для заданного шага.
        /// https://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf (Страница 2).
        /// </summary>
        /// <param name="t">Шаг.</param>
        /// <param name="p0">Полюс 1.</param>
        /// <param name="p1">Полюс 2.</param>
        /// <param name="p2">Полюс 3.</param>
        /// <param name="p3">Полюс 4.</param>
        /// <returns>Вектор.</returns>
        private static Vector2 ReturnCarmullRomVector(double dt, double aTension, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) {
            Vector2 a = p1;
            Vector2 b = aTension * (p2 - p0);
            Vector2 c = 3f * (p2 - p1) - aTension * (p3 - p1) - 2f * aTension * (p2 - p0);
            Vector2 d = -2f * (p2 - p1) + aTension * (p3 - p1) + aTension * (p2 - p0);

            Vector2 result = a + (b * dt) + (c * dt * dt) + (d * dt * dt * dt);

            return result;
        }
    }
}
