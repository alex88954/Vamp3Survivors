using System.Collections;
// Classe abstraite pour les améliorations d'attaque et les attaques elles-mêmes
public abstract class Attaque
{
    protected ScriptAttaque scriptAttaque;
    public virtual bool EstDebloquee { get; set; } = true;

    public Attaque(ScriptAttaque scriptAttaque)
    {
        this.scriptAttaque = scriptAttaque;
    }
    public abstract void OnRayonAmeliore();
    public abstract void OnNiveauAttaqueAmeliore();
    public abstract IEnumerator Attaquer();





}
