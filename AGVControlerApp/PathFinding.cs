using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lego.Ev3.Core;

namespace AGVControlerApp
{
    class PathFinding
    {
        Brick brick;

        Track black;
        Track yellow;
        Track red;
        Track white;
        Track whiteYellow;
        Track whiteRed;
        Track currentTrack;
        Track theOtherBlack;

        Track parent;



        public PathFinding(Brick brick)
        {
            this.brick = brick;
            black = new Track(Color.BLACK);
            yellow = new Track(Color.YELLOW, black);
            red = new Track(Color.RED, black);
            white = new Track(Color.WHITE, black);
            whiteYellow = new Track(Color.WHITE, yellow);
            whiteRed = new Track(Color.WHITE, red);
            theOtherBlack = new Track(Color.OTHERBLACK);
        }

        public void solve()
        {
            currentTrack = new Track((Color)(Convert.ToInt32(brick.Ports[InputPort.One].SIValue)), parent);


            currentTrack.setColor((Color)(Convert.ToInt32(brick.Ports[InputPort.One].SIValue)));

            if (currentTrack.getColor() == black.getColor() || currentTrack.getColor() == theOtherBlack.getColor())
            {
                moveForward();
                parent = null;
            }

            if (currentTrack.getParent() == yellow && currentTrack.getColor() == white.getColor())
            {
                moveSharpRight();
            }
            if (currentTrack.getParent() == red && currentTrack.getColor() == white.getColor())
            {
                moveSharpLeft();
            }

            if (currentTrack.getColor() == yellow.getColor())
            {
                parent = yellow;
                moveRight();
            }
            if (currentTrack.getColor() == red.getColor())
            {
                parent = red;
                moveLeft();
            }

            Console.WriteLine(currentTrack.getColor());
        }

        private async void moveSharpLeft()
        {
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, -20, 100, false);
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, 20, 100, false);
            await brick.BatchCommand.SendCommandAsync();
        }


        private async void moveSharpRight()
        {
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, -20, 100, false);
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, 20, 100, false);
            await brick.BatchCommand.SendCommandAsync();
        }

        private async void moveLeft()
        {
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, -5, 100, false);
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, 20, 100, false);
            await brick.BatchCommand.SendCommandAsync();
        }

        private async void moveRight()
        {
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.D, -5, 100, false);
            brick.BatchCommand.TurnMotorAtPowerForTime(OutputPort.A, 20, 100, false);
            await brick.BatchCommand.SendCommandAsync();
        }

        private async void moveForward()
        {
            await brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A | OutputPort.D, 20, 100, false);
        }
    }
}
