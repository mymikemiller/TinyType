using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyTypeLib
{
    public struct InputAction
    {
        public enum ActionType { None, TypeCharacter, ForwardDelete, Backspace, DeleteWord, Upper, Lower, Characters, Symbols }
        public ActionType Type;

        // The character to be displayed on the InputView. Only typed when chosen if Type is TypeCharacter; for other ActionTypes, this is only for display purposes.
        public char Character;

        public InputAction(ActionType actionType)
        {
            Character = new char();
            Type = actionType;
        }
        /// <summary>
        /// Initializes an Action of type TypeCharacter for the specified character.
        /// </summary>
        /// <param name="Character">The character to be typed</param>
        public InputAction(char Character)
        {
            Type = ActionType.TypeCharacter;
            this.Character = Character;
        }

    }
}
