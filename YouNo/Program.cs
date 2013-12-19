// <copyright file="Program.cs" company="Spruntheus Games">
//      All rights reserved.
// </copyright>
// <author>Dylan Wang</author>
namespace YouNo
{
    /// <summary>
    /// Main Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry of program
        /// </summary>
        /// <param name="args">Arguments ignored</param>
        private static void Main(string[] args)
        {
            YouNoGame game = new YouNoGame();
            game.Run();
        }
    }
}
