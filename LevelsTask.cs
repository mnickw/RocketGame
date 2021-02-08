using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		public static IEnumerable<Level> CreateLevels()
		{
			yield return GetLevelWithStandartPhisicsRocketAndTarget("Zero", (size, v) => Vector.Zero);
			yield return GetLevelWithStandartPhisicsRocketAndTarget("Heavy", (size, v) => new Vector(0, 0.9));
			yield return GetLevelWithStandartPhisicsAndRocket("Up", new Vector(700, 500), (size, v) => new Vector(0, -(300 / (size.Height - v.Y + 300.0))));
			yield return GetLevelWithStandartPhisicsRocketAndTarget("WhiteHole", (size, v) => GetGravityForceForWhiteHole(v));
			yield return GetLevelWithStandartPhisicsRocketAndTarget("BlackHole", (size, v) => GetGravityForceForBlackHole(v));
			yield return GetLevelWithStandartPhisicsRocketAndTarget("BlackAndWhite", (size, v) =>
			{
				var black = GetGravityForceForBlackHole(v);
				var white = GetGravityForceForWhiteHole(v);
				return new Vector((black.X + white.X) / 2, (black.Y + white.Y) / 2);
			});
		}

		private static readonly Vector standardTargetLocation = new Vector(600, 200);
		private static readonly Vector standardRocketLocation = new Vector(200, 500);
		private static readonly Physics standardPhysics = new Physics();
		private static readonly Vector standardAnomalyLocation =
			new Vector((standardTargetLocation.X - standardRocketLocation.X) / 2 + standardRocketLocation.X,
					   (standardRocketLocation.Y - standardTargetLocation.Y) / 2 + standardTargetLocation.Y);

		private static Rocket GetStandartRocket() => new Rocket(standardRocketLocation, Vector.Zero, -0.5 * Math.PI);

		private static Level GetLevelWithStandartPhisicsAndRocket(string name, Vector target, Gravity gravity) =>
			new Level(name, GetStandartRocket(), target, gravity, standardPhysics);

		private static Level GetLevelWithStandartPhisicsRocketAndTarget(string name, Gravity gravity) =>
			GetLevelWithStandartPhisicsAndRocket(name, standardTargetLocation, gravity);

		private static Vector GetGravityForceForHoles(Vector first, Vector second, double k)
		{
			double d = Math.Sqrt((Math.Pow((second.X - first.X), 2) + Math.Pow((second.Y - first.Y), 2)));
			double absDir = k * d / (Math.Pow(d, 2) + 1);
			double x, y;
			if (second.X == first.X && second.Y == first.Y)
			{
				x = second.X * absDir;
				y = second.Y * absDir;
			}
			else
			{
				x = (second.X - first.X) / d * absDir;
				y = (second.Y - first.Y) / d * absDir;
			}
			return new Vector(x, y);
		}

		private static Vector GetGravityForceForBlackHole(Vector location) => GetGravityForceForHoles(location, standardAnomalyLocation, 300);

		private static Vector GetGravityForceForWhiteHole(Vector location) => GetGravityForceForHoles(standardTargetLocation, location, 140);
	}
}