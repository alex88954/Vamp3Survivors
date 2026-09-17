using UnityEngine;

public class EtatPoursuite : EtatEnnemi
{
    public EtatPoursuite(ComportementEnnemi comportementEnnemi) : base(comportementEnnemi)
    { }

    public override void Entrer()
    {
        sujet.agent.SetDestination(sujet.cibleCourante.position);
        sujet.agent.isStopped = false;
    }

    public override void Executer(float deltaTime)
    {
        Vector3 positionCible = sujet.cibleCourante.position;
        Vector3 positionEnnemi = sujet.gameObject.transform.position;
        float distanceJoueur = (positionCible - positionEnnemi).magnitude;

        if (distanceJoueur < sujet.distanceAttaqueToPoursuite)
        {
            sujet.ChangerEtat(sujet.etatAttaque);
        }
        else
        {
            sujet.agent.SetDestination(positionCible);
        }
    }
}
