using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace FPCTweaks.Patches;

public class AutoLook : IScriptMod
{
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
    {
        // mouse_look = Input.is_action_pressed("mouse_look")
        var waiter_cam = new MultiTokenWaiter([
            t => t is IdentifierToken{Name:"mouse_look"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken{Name:"Input"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken{Name:"is_action_pressed"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken{Value:StringVariant { Value:"mouse_look" }},
            t => t.Type is TokenType.ParenthesisClose,
        ], allowPartialMatch: false);

        foreach (var token in tokens)
        {
            if (waiter_cam.Check(token))
            {
                yield return token;
                yield return new Token(TokenType.Newline, 1);
                
                // if camera_zoom <= 0.0:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("camera_zoom");
                yield return new Token(TokenType.OpLessEqual);
                yield return new ConstantToken(new RealVariant(0.0));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                // mouse_look = true
                    yield return new IdentifierToken("mouse_look");
                    yield return new Token(TokenType.OpAssign);
                    yield return new ConstantToken(new BoolVariant(true));
                
                    yield return new Token(TokenType.Newline, 1);
            } else yield return token;
        }
    }
}