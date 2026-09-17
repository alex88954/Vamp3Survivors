public class ParametresJeu
{
    public static ParametresJeu Instance { get; } = new ParametresJeu();

    public int VieDepart { get; private set; } = 14;
    public int ExperienceRequise { get; private set; } = 30;
    public int EnnemisParVague { get; private set; } = 5;

    public enum Modes
    {
        Facile,
        Difficile
    }
    public Modes modeCourant { get; private set; } = Modes.Facile;

    public StrategieApparitionVague[] strategiesApparitionVague = new StrategieApparitionVague[]
    {
     new StrategieApparitionHasard()
    };

    private ParametresJeu()
    {
    }

    public void ModeFacile()
    {
        modeCourant = Modes.Facile;
        VieDepart = 14;
        ExperienceRequise = 30;
        EnnemisParVague = 5;
        strategiesApparitionVague = new StrategieApparitionVague[]
        {
            new StrategieApparitionHasard(),
        };
    }

    public void ModeDifficile()
    {
        modeCourant = Modes.Difficile;
        VieDepart = 8;
        ExperienceRequise = 50;
        EnnemisParVague = 10;
        strategiesApparitionVague = new StrategieApparitionVague[]{
            new StrategieApparitionHasard(),
            new StrategieApparitionLigne(),
            new StrategieApparitionCercle(),
    };
    }
}