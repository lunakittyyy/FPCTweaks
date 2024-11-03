using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace FPCTweaks.Patches;

public class SoftLockPatch : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Menus/Main Menu/main_menu.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // func _ready():
        var waiter_lock = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken{Name:"_ready"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Colon,
        ], allowPartialMatch: false);

        foreach (var token in tokens)
        {
            if (waiter_lock.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 1);
                
                // Input.set_mouse_mode(0)
                yield return new IdentifierToken("Input");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("set_mouse_mode");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new RealVariant(0));
                yield return new Token(TokenType.ParenthesisClose);
            
                yield return new Token(TokenType.Newline, 1);
            } else yield return token;
        }
    }
}