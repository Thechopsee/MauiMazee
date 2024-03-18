using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMaze.Drawables;
using MauiMaze.Helpers;
using MauiMaze.Models;
using MauiMaze.Models.ClassicMaze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMaze.ViewModels
{
    public enum MazeType
    {
        Classic,
        Rounded,
    }
    public partial class MazeMenuViewModel :ObservableObject
    {
        private GraphicsView canvas;
        private MazeType actual;
        private GeneratorEnum generator;
        private Button classic;
        private Button rounded;
        private Color native;
        private Color clicked;
        private LoginCases login;
        [ObservableProperty]
        int mazesize;
        [ObservableProperty]
        Color setsColor;
        [ObservableProperty]
        Color huntColor;
        public MazeMenuViewModel(GraphicsView canvas,Button classic,Button rounded, LoginCases login)
        {
            this.login = login;
            this.canvas = canvas;
            Mazesize = 10;
            this.classic = classic;
            this.rounded = rounded;
            clicked = classic.BackgroundColor;
            native = rounded.BackgroundColor;
            setsColor= classic.BackgroundColor;
            huntColor= rounded.BackgroundColor;
            actual = MazeType.Classic;
            generator = GeneratorEnum.Sets;
            MazeDrawable md = new MazeDrawable();
            Maze maze = new Maze(Mazesize, Mazesize,generator);
            md.maze = maze;
            this.canvas.Drawable = md;
            this.canvas.Invalidate();
        }
        [RelayCommand]
        public void roundedClick()
        {
            canvas.Drawable = new RoundedMazeDrawable(generator);
            actual = MazeType.Rounded;
            classic.BackgroundColor = native;
            rounded.BackgroundColor = clicked;
        }
        [RelayCommand]
        public void classicClick()
        {
            MazeDrawable md = new MazeDrawable();
            md.maze = new Maze(Mazesize, Mazesize, generator);
            canvas.Drawable = md;
            actual = MazeType.Classic;
            classic.BackgroundColor = clicked;
            rounded.BackgroundColor = native;
        }
        [RelayCommand]
        public void setsClick()
        {
            generator = GeneratorEnum.Sets;
            if (actual == MazeType.Classic)
            {
                MazeDrawable md = new MazeDrawable();
                md.maze = new Maze(Mazesize, Mazesize, generator);
                canvas.Drawable = md;
            }
            else
            {
                canvas.Drawable = new RoundedMazeDrawable(generator);
            }

            HuntColor = native;
            SetsColor = clicked;
        }
        [RelayCommand]
        public void huntClick()
        {
            generator = GeneratorEnum.HuntNKill;
            if (actual == MazeType.Classic)
            {
                MazeDrawable md = new MazeDrawable();
                md.maze = new Maze(Mazesize, Mazesize,generator);
                canvas.Drawable = md;
            }
            else
            {
                canvas.Drawable = new RoundedMazeDrawable(generator);
            }
            HuntColor = clicked;
            SetsColor = native;
        }

        [RelayCommand]
        private async Task mazeMenuNavigate()
        {
            switch (actual)
            {
                case MazeType.Classic:
                    await Shell.Current.Navigation.PushAsync(new MazePage(Mazesize,generator));
                    return;
                case MazeType.Rounded:
                    await Shell.Current.Navigation.PushAsync(new RoundedMazePage(Mazesize, generator));
                    return;
            }

        }

        [RelayCommand]
        private void dragMazeEnded()
        {
            if (actual == MazeType.Classic)
            {
                MazeDrawable md = new MazeDrawable();
                Maze maze = new Maze(Mazesize, Mazesize, generator);
                md.maze = maze;
                canvas.Drawable = md;
                canvas.Invalidate();
            }
            else
            {
                canvas.Drawable = new RoundedMazeDrawable(generator);
                canvas.Invalidate();
            }
            
        }
    }
}
