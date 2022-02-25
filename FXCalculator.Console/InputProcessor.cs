using System.Text.RegularExpressions;

public class InputProcessor
{
    private string _input;

    /// <summary>
    /// A utility class to process players' input
    /// </summary>
    /// <param name="input"></param>
    public InputProcessor(string input)
    {
        _input = input.ToUpper();
    }

    /// <summary>
    /// Validate to ensure only numbers, letters and spaces have been entered
    /// </summary>
    /// <returns></returns>
    public bool Validate()
    {
        Regex rg = new Regex(@"^[0-9A-Za-z ]+$");
        var regexMatch = rg.Match(_input);
        
        return regexMatch.Success;
    }

    public 
    
    /// <summary>
    /// Process the input entered to setup the grid/arena
    /// </summary>
    /// <returns></returns>
    public IInputValidationResult ProcessInitializeGridInstruction()
    {
        GridInitializationValidationResult validationResult = new GridInitializationValidationResult();
        
        Regex rg = new Regex(@"^[0-9]+\s[0-9]+$"); //Regex to match the format 5 5

        if (rg.IsMatch(_input))
        {
            string[] input = _input.Split(" ");
            if (input != null)
            {
                bool parseResult = false;
                
                parseResult = Int32.TryParse(input[0], out int width);
                parseResult = Int32.TryParse(input[1], out int height);

                if (parseResult)
                {
                    GridSize gridSize = new GridSize(width, height);
                    var gridSizeValidationResult = gridSize.Validate();

                    if (gridSizeValidationResult.Success)
                    {
                        validationResult.Success = true;
                        validationResult.GridSize = gridSize;    
                    }
                    else
                    {
                        validationResult.ErrorMessage = gridSizeValidationResult.ErrorMessage;
                    }
                }
            }
        }

        if (!validationResult.Success && string.IsNullOrEmpty(validationResult.ErrorMessage))
            validationResult.ErrorMessage = "Grid initialization instruction is in incorrect format. Please enter the size as - {Width} {Height}. Eg : 5 5";

        return validationResult;
    }
    
    /// <summary>
    /// Process the input entered to add a robot
    /// Validate to ensure the input is in the correct format Eg : 1 2 N
    /// </summary>
    /// <returns></returns>
    public IInputValidationResult ProcessAddInstruction()
    {
        Regex rg = new Regex(@"^[0-9]+\s[0-9]+ [a-zA-Z]$"); //Regex to match the format - 1 2 N
        RobotAddInstructionValidationResult validationResult = new RobotAddInstructionValidationResult();

        if (rg.IsMatch(_input))
        {
            string[] input = _input.Split(" ");
            Int32.TryParse(input[0], out int x);
            Int32.TryParse(input[1], out int y);
            var bearing = ConvertBearing(input[2]);
            if (bearing != null)
            {
                Position position = new Position(x, y, bearing.Value);
                validationResult.Success = true;
                validationResult.Position = position;
            }
            else
            {
                validationResult.ErrorMessage = "Incorrect direction entered. Direction can only be entered as one of the following : N, E, W or S";
            }
        }
        else
        {
            validationResult.Success = false;
            validationResult.ErrorMessage = "Robot add instruction is in incorrect format. Please enter instruction as {X Position} {Y Position} {Direction - N/W/E/S}. Eg : 1 2 N";
        }

        return validationResult;
    }

    /// <summary>
    /// Converts the entered character for the direction to an enum value
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private BearingType? ConvertBearing(string c)
    {
        BearingType? bearingType = c switch
        {
            "N" => BearingType.North,
            "E" => BearingType.East,
            "S" => BearingType.South,
            "W" => BearingType.West,
            _ => null
        };

        return bearingType;
    }



    /// <summary>
    /// Process the input entered to move a robot
    /// Validate to ensure the input is in the correct format Eg : LRMLLLMRRR
    /// </summary>
    /// <returns></returns>
    public IInputValidationResult ProcessMoveInstruction()
    {
        ICollection<MovementInstructionType> instructions = new List<MovementInstructionType>();
        MovementInstructionValidationResult validationResult = new MovementInstructionValidationResult();
        
        char[] charArray = _input.ToUpper().ToCharArray();
        if (charArray != null)
        {
            bool exitLoop = false;

            foreach (char character in charArray)
            {
                switch (character)
                {
                    case 'L':
                        instructions.Add(MovementInstructionType.RotateLeft);
                        break;
                    case 'R':
                        instructions.Add(MovementInstructionType.RotateRight);
                        break;
                    case 'M':
                        instructions.Add(MovementInstructionType.MoveForward);
                        break;
                    default: //If an unrecognized character has been entered, inform the player the correct format
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "Movement instructions are in incorrect format. Possible characters are 'L', 'R' to spin the robot 90 degrees, and 'M' to move the robot one grid point forward. eg : LRMLLLMRRR";
                        exitLoop = true;
                        break;
                }
                
                if (exitLoop) break;
            }

            if (!exitLoop)
            {
                validationResult.Success = true;
                validationResult.MovementInstructions = instructions;    
            }
            
            return validationResult;
        }
            

        return validationResult;
    }
}