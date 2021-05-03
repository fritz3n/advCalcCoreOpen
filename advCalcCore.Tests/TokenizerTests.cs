using advCalcCore.Tokenizing;
using advCalcCore.Tokenizing.Tokens;
using System.Collections.Generic;
using Xunit;

namespace advCalcCore.Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void TokenizeEmpty()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("", 0, 0);
            Assert.Empty(tokenized);

            tokenized = tokenizier.Tokenize("          ", 0, 0); // some Whitespace
            Assert.Empty(tokenized);

            tokenized = tokenizier.Tokenize("\n", 0, 0); // a newline should result in an instructSeperator
            Assert.Collection(tokenized, item =>
                {
                    Assert.Equal("instructSeperator", item.Name);
                    Assert.Equal("\n", item.Text);
                    Assert.Equal(0, item.Range.Start);
                    Assert.Equal(1, item.Range.End);
                    Assert.Equal(Token.TokenType.None, item.Type);
                }
                );
        }

        [Fact]
        public void TokenizeCalculations()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;
            
            // Calculation
            tokenized= tokenizier.Tokenize("1+2", 0, 0);
            Assert.Collection(tokenized, item => Assert.Equal("1", item.Text), item => Assert.Equal("+", item.Text), item => Assert.Equal("2", item.Text));

            // Calculation
            tokenized = tokenizier.Tokenize("a=0\na++", 0, 0);
            Assert.Collection(tokenized, 
                item => Assert.Equal("a", item.Text), 
                item => Assert.Equal("=", item.Text), 
                item => Assert.Equal("0", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("++", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("a=0\na--", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("0", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("--", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("a=0\na-=2", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("0", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("-=", item.Text),
                item => Assert.Equal("2", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("a=0\na+=2", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("0", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("+=", item.Text),
                item => Assert.Equal("2", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("a=23\na*=2", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("23", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("*=", item.Text),
                item => Assert.Equal("2", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("a=4\na/=2", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("4", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("/=", item.Text),
                item => Assert.Equal("2", item.Text)
                );

            // Calculation
            tokenized = tokenizier.Tokenize("1*22", 0, 0);
            Assert.Collection(tokenized, item => Assert.Equal("1", item.Text), item => Assert.Equal("*", item.Text), item => Assert.Equal("22", item.Text));

            // Whitespace: Space
            tokenized = tokenizier.Tokenize(" 1 + 2 ", 0, 0);
            Assert.Collection(tokenized, item => Assert.Equal("1", item.Text), item => Assert.Equal("+", item.Text), item => Assert.Equal("2", item.Text));

            // More Whitespace
            tokenized = tokenizier.Tokenize(" 1     +   2 ", 0, 0);
            Assert.Collection(tokenized, item => Assert.Equal("1", item.Text), item => Assert.Equal("+", item.Text), item => Assert.Equal("2", item.Text));

            // Calculation
            tokenized = tokenizier.Tokenize("1+2-33*4/5+5!", 0, 0);
            Assert.Collection(tokenized, item => Assert.Equal("1", item.Text), item => Assert.Equal("+", item.Text), item => Assert.Equal("2", item.Text),
                item => Assert.Equal("-", item.Text), item => Assert.Equal("33", item.Text), item => Assert.Equal("*", item.Text), 
                item => Assert.Equal("4", item.Text), item => Assert.Equal("/", item.Text), item => Assert.Equal("5", item.Text),
                item => Assert.Equal("+", item.Text), item => Assert.Equal("5", item.Text), item => Assert.Equal("!", item.Text));
        }

        [Fact]
        public void TokenizeSqrt()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("sqrt(16)", 0, 0);
            Assert.Collection(tokenized, 
                item => Assert.Equal("sqrt", item.Text), 
                item => {
                Assert.Equal("(", ((CompoundToken)item).Opening.ToString());
                Assert.Equal(")", ((CompoundToken)item).Closing.ToString());
                Assert.Collection(((CompoundToken)item).Tokens,
                    inside => Assert.Equal("16", inside.Text));
                }
            );
        }

        [Fact]
        public void TokenizeBrackets()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("(1983.3+32)", 0, 0);
            Assert.Collection(tokenized, item =>
            {
                Assert.Equal("(", ((CompoundToken)item).Opening.ToString());
                Assert.Equal(")", ((CompoundToken)item).Closing.ToString());
                Assert.Collection(((CompoundToken)item).Tokens, 
                    inside => Assert.Equal("1983.3", inside.Text), 
                    inside => Assert.Equal("+", inside.Text), 
                    inside => Assert.Equal("32", inside.Text));
            });

            tokenized = tokenizier.Tokenize("8*(1983.3+32)", 0, 0);
            Assert.Collection(tokenized, 
                item => Assert.Equal("8", item.Text),
                item => Assert.Equal("*", item.Text),
                item => {
                    Assert.Equal("(", ((CompoundToken)item).Opening.ToString());
                    Assert.Equal(")", ((CompoundToken)item).Closing.ToString());
                    Assert.Collection(((CompoundToken)item).Tokens,
                        inside => Assert.Equal("1983.3", inside.Text),
                        inside => Assert.Equal("+", inside.Text),
                        inside => Assert.Equal("32", inside.Text));
                    }
                );

            tokenized = tokenizier.Tokenize("8*(1983.3+32+(3/7))", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("8", item.Text),
                item => Assert.Equal("*", item.Text),
                item => {
                    Assert.Equal("(", ((CompoundToken)item).Opening.ToString());
                    Assert.Equal(")", ((CompoundToken)item).Closing.ToString());
                    Assert.Collection(((CompoundToken)item).Tokens,
                        inside => Assert.Equal("1983.3", inside.Text),
                        inside => Assert.Equal("+", inside.Text),
                        inside => Assert.Equal("32", inside.Text),
                        inside => Assert.Equal("+", inside.Text),
                        inside => {
                            Assert.Equal("(", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal(")", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("3", ininside.Text),
                                ininside => Assert.Equal("/", ininside.Text),
                                ininside => Assert.Equal("7", ininside.Text));
                            }
                        );
                    }
                );
        }

        [Fact]
        public void TokenizeIf()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("if(a < b) {a = 23; b = 3.3;} \n d = \"Text\"", 0, 0);
            Assert.Collection(tokenized, 
                item =>
                {
                    Assert.Equal("if", ((ConstructToken)item).Name);
                    Assert.Collection(((ConstructToken)item).Tokens,
                        inside =>
                        {
                            Assert.Equal("if", inside.Text);
                        },
                        inside =>
                        {
                            Assert.Equal("(", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal(")", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal("<", ininside.Text),
                                ininside => Assert.Equal("b", ininside.Text)
                                );
                        },
                        inside =>
                        {
                            Assert.Equal("{", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal("}", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal("=", ininside.Text),
                                ininside => Assert.Equal("23", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text),
                                ininside => Assert.Equal("b", ininside.Text),
                                ininside => Assert.Equal("=", ininside.Text),
                                ininside => Assert.Equal("3.3", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text)
                                );
                        }
                    );
                },
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("d", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("Text", item.Text)
            );
        }

        [Fact]
        public void TokenizeFor()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("a=2\nfor(i=0;i<5;i++) {a++;}\nd=\"Text\"\na", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("2", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item =>
                {
                    Assert.Equal("for", ((ConstructToken)item).Name);
                    Assert.Collection(((ConstructToken)item).Tokens,
                        inside =>
                        {
                            Assert.Equal("for", inside.Text);
                        },
                        inside =>
                        {
                            Assert.Equal("(", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal(")", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("i", ininside.Text),
                                ininside => Assert.Equal("=", ininside.Text),
                                ininside => Assert.Equal("0", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text),
                                ininside => Assert.Equal("i", ininside.Text),
                                ininside => Assert.Equal("<", ininside.Text),
                                ininside => Assert.Equal("5", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text),
                                ininside => Assert.Equal("i", ininside.Text),
                                ininside => Assert.Equal("++", ininside.Text)
                                );
                        },
                        inside =>
                        {
                            Assert.Equal("{", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal("}", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal("++", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text)
                                );
                        }
                    );
                },
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("d", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("Text", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text)
            );
        }

        [Fact]
        public void TokenizeWhile()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("a=2\nb=5\nwhile(a < b) {a++;}\nd=\"Text\"\na", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("a", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("2", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("b", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("5", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item =>
                {
                    Assert.Equal("while", ((ConstructToken)item).Name);
                    Assert.Collection(((ConstructToken)item).Tokens,
                        inside =>
                        {
                            Assert.Equal("while", inside.Text);
                        },
                        inside =>
                        {
                            Assert.Equal("(", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal(")", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal("<", ininside.Text),
                                ininside => Assert.Equal("b", ininside.Text)
                                );
                        },
                        inside =>
                        {
                            Assert.Equal("{", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal("}", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal("++", ininside.Text),
                                ininside => Assert.Equal(";", ininside.Text)
                                );
                        }
                    );
                },
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("d", item.Text),
                item => Assert.Equal("=", item.Text),
                item => Assert.Equal("Text", item.Text),
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("a", item.Text)
            );
        }

        [Fact]
        public void TokenizeLambda()
        {
            Tokenizer tokenizier = new Tokenizer();
            List<Token> tokenized;

            tokenized = tokenizier.Tokenize("e=(a,b)=>{return(a+b)}\ne(1,2)", 0, 0);
            Assert.Collection(tokenized,
                item => Assert.Equal("e", item.Text),
                item => Assert.Equal("=", item.Text),
                item =>
                {
                    Assert.Equal("lambda", ((ConstructToken)item).Name);
                    Assert.Collection(((ConstructToken)item).Tokens,
                        inside =>
                        {
                            Assert.Equal("(", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal(")", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("a", ininside.Text),
                                ininside => Assert.Equal(",", ininside.Text),
                                ininside => Assert.Equal("b", ininside.Text)
                                );
                        },
                        inside => 
                            {
                                Assert.Equal("(a,b)=>{return(a+b)}", item.Text);
                                Assert.Equal("lambda", item.Name);
                            },
                        inside =>
                        {
                            Assert.Equal("{", ((CompoundToken)inside).Opening.ToString());
                            Assert.Equal("}", ((CompoundToken)inside).Closing.ToString());
                            Assert.Collection(((CompoundToken)inside).Tokens,
                                ininside => Assert.Equal("return", ininside.Text),
                                ininside =>
                                    {
                                        Assert.Equal("(", ((CompoundToken)ininside).Opening.ToString());
                                        Assert.Equal(")", ((CompoundToken)ininside).Closing.ToString());
                                        Assert.Collection(((CompoundToken)ininside).Tokens,
                                            inininside => Assert.Equal("a", inininside.Text),
                                            inininside => Assert.Equal("+", inininside.Text),
                                            inininside => Assert.Equal("b", inininside.Text)
                                            );
                                    }
                                );
                        }
                    );
                },
                item => Assert.Equal("instructSeperator", item.Name),
                item => Assert.Equal("e", item.Text),
                item =>
                {
                    Assert.Equal("(", ((CompoundToken)item).Opening.ToString());
                    Assert.Equal(")", ((CompoundToken)item).Closing.ToString());
                    Assert.Collection(((CompoundToken)item).Tokens,
                        ininside => Assert.Equal("1", ininside.Text),
                        ininside => Assert.Equal(",", ininside.Text),
                        ininside => Assert.Equal("2", ininside.Text)
                        );
                }
            );
        }

        [Fact]
        public void TokenizeWithErrors()
        {
            Tokenizer tokenizier = new Tokenizer();
            Assert.Throws<TokenizeException>(() => tokenizier.Tokenize("if 23", 0, 0));
            Assert.Throws<TokenizeException>(() => tokenizier.Tokenize("+", 0, 0));
            Assert.Throws<TokenizeException>(() => tokenizier.Tokenize("(8")); // "No closing bracket..."
            Assert.Throws<TokenizeException>(() => tokenizier.Tokenize("8)")); // "Unknown char"
        }

    }
}
