using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;

// Classe que representa um único triângulo na grade
public class TriangleCell
{
    public Vector2 A, B, C;
    public bool IsBuilt = false;
    public static readonly Color DefaultColor = new Color(60, 100, 160, 255); // Cor base para todos (Azul Escuro)
    public static readonly Color BuiltColor = new Color(200, 160, 60, 255);   // Cor ao ser clicado (Laranja)

    public TriangleCell(Vector2 a, Vector2 b, Vector2 c)
    {
        A = a; B = b; C = c;
    }

    // Lógica Contains: Verifica se um ponto (p) está dentro do triângulo
    public bool Contains(Vector2 p)
    {
        var v0 = C - A;
        var v1 = B - A;
        var v2 = p - A;

        var dot00 = Vector2.Dot(v0, v0);
        var dot01 = Vector2.Dot(v0, v1);
        var dot02 = Vector2.Dot(v0, v2);
        var dot11 = Vector2.Dot(v1, v1);
        var dot12 = Vector2.Dot(v1, v2);

        var invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }

    public Color CurrentColor => IsBuilt ? BuiltColor : DefaultColor;
}

// Classe principal do jogo (Core do Raylib)
public class Game
{
    private const int ScreenWidth = 1200;
    private const int ScreenHeight = 800;
    private const float SideLength = 50f;
    // Altura do triângulo equilátero: s * sqrt(3) / 2
    private static readonly float Height = SideLength * (float)Math.Sqrt(3) / 2; 

    // Passo horizontal (SideLength/2)
    private static readonly float TriangleGridStepX = SideLength / 2; 
    // Passo vertical (Height)
    private static readonly float TriangleGridStepY = Height; 

    private List<TriangleCell> _triangles = new();

    public void Run()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Grade Triangular Hexagonal (Finalizada e Completa)");
        Raylib.SetTargetFPS(60);

        GenerateFullGrid();

        while (!Raylib.WindowShouldClose())
        {
            HandleInput();
            Render();
        }

        Raylib.CloseWindow();
    }

    private void GenerateFullGrid()
    {
        _triangles.Clear();
        
        // Ajuste para garantir que a grade comece ANTES de x=0 e y=0, eliminando espaços pretos nas bordas.
        float xStartOffset = -SideLength; 
        float yStartOffset = -Height;     

        // Número de "colunas" e "linhas" aumentados para cobrir o offset e a tela.
        int numCols = (int)(ScreenWidth / SideLength) * 2 + 8;
        int numRows = (int)(ScreenHeight / Height) + 5;

        for (int row = 0; row < numRows; row++)
        {
            // 1. Posição Y da linha base, incluindo o offset inicial.
            float yBase = row * TriangleGridStepY + yStartOffset;

            // 2. Deslocamento X da linha (a cada linha ímpar, desloca por SideLength/2)
            float xRowOffset = (row % 2 == 1) ? TriangleGridStepX : 0; 

            for (int col = 0; col < numCols; col++)
            {
                // Posição X base do triângulo, incluindo offset e deslocamento de linha.
                float xBase = col * TriangleGridStepX + xRowOffset + xStartOffset;

                // A orientação do triângulo alterna a cada passo horizontal
                bool isUp = (col % 2 == 0); 

                if (isUp)
                {
                    // ▲ (Para cima) - Ordem Anti-horária (CCW)
                    var A = new Vector2(xBase, yBase + Height);         // Inferior Esquerdo
                    var B = new Vector2(xBase + SideLength, yBase + Height); // Inferior Direito
                    var C = new Vector2(xBase + TriangleGridStepX, yBase); // Topo Central
                    
                    _triangles.Add(new TriangleCell(A, B, C));
                }
                else
                {
                    // ▽ (Para baixo) - Ordem Anti-horária (CCW)
                    // Garante que o preenchimento seja desenhado corretamente
                    var A = new Vector2(xBase, yBase);                       // Topo Esquerdo
                    var B = new Vector2(xBase + TriangleGridStepX, yBase + Height); // Fundo Central
                    var C = new Vector2(xBase + SideLength, yBase);          // Topo Direito
                    
                    _triangles.Add(new TriangleCell(A, B, C));
                }
            }
        }
    }

    private void HandleInput()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            var mouse = Raylib.GetMousePosition();
            // Itera sobre todos os triângulos para verificar o clique
            foreach (var tri in _triangles)
            {
                if (tri.Contains(mouse))
                {
                    tri.IsBuilt = !tri.IsBuilt; // Alterna o estado (cor)
                    break;
                }
            }
        }
    }

    private void Render()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black); 

        foreach (var tri in _triangles)
        {
            // Desenha o preenchimento (agora aparecerá para todos graças à ordem CCW)
            Raylib.DrawTriangle(tri.A, tri.B, tri.C, tri.CurrentColor);
            // Desenha a linha
            Raylib.DrawTriangleLines(tri.A, tri.B, tri.C, Color.White); 
        }

        Raylib.DrawText("Clique em qualquer triângulo! (Grade Hexagonal Finalizada)", 20, 20, 20, Color.White);
        Raylib.EndDrawing();
    }
}

// Classe de entrada para o programa
public class Program
{
    public static void Main()
    {
        new Game().Run();
    }
}