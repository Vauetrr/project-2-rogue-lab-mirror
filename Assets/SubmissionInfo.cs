
// This class contains metadata for your submission. It plugs into some of our
// grading tools to extract your game/team details. Ensure all Gradescope tests
// pass when submitting, as these do some basic checks of this file.
public static class SubmissionInfo
{
    // TASK: Fill out all team + team member details below by replacing the
    // content of the strings. Also ensure you read the specification carefully
    // for extra details related to use of this file.

    // URL to your group's project 2 repository on GitHub.
    public static readonly string RepoURL = "https://github.com/COMP30019/project-2-rogue-lab";
    
    // Come up with a team name below (plain text, no more than 50 chars).
    public static readonly string TeamName = "Rogue Lab";
    
    // List every team member below. Ensure student names/emails match official
    // UniMelb records exactly (e.g. avoid nicknames or aliases).
    public static readonly TeamMember[] Team = new[]
    {
        new TeamMember("Changmin Lee", "changminl@student.unimelb.edu.au"),
        new TeamMember("Ishaann Cheema", "icheema@student.unimelb.edu.au"),
        new TeamMember("Shuaixian Li", "shuaixianl@student.unimelb.edu.au"),
        new TeamMember("Max Semmler", "msemmler@student.unimelb.edu.au"), 
    };

    // This may be a "working title" to begin with, but ensure it is final by
    // the video milestone deadline (plain text, no more than 50 chars).
    public static readonly string GameName = "Downfall";

    // Write a brief blurb of your game, no more than 200 words. Again, ensure
    // this is final by the video milestone deadline.
    public static readonly string GameBlurb = 
@"The queen is dead. The kingdom is in ruin, and you've been framed for the murder. 
Try your hand at escaping the palace dungeon in this isometric roguelite game. Get stronger
each run and gain abilities as you learn from your predecessor's mistakes. Navigate
procedurally generated dungeons and slay the final boss to escape once and for all.";
    
    // By the gameplay video milestone deadline this should be a direct link
    // to a YouTube video upload containing your video. Ensure "Made for kids"
    // is turned off in the video settings. 
    public static readonly string GameplayVideo = "https://www.youtube.com/watch?v=-Q1e_Zr7q2o";
    
    // No more info to fill out!
    // Please don't modify anything below here.
    public readonly struct TeamMember
    {
        public TeamMember(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; }
        public string Email { get; }
    }
}
