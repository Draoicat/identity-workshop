using System;

namespace Core
{
    public enum Parties : int
    {
        Optimism = 0,
        Shyness = 1,
        AI = 2,
        Selfless = 3,
        Anger = 4
    }

    internal static class PartiesExtensions 
    {
        public static string PartyDescription(this Parties party)
        {
            return party switch
            {
                Parties.Optimism => "C’est votre personnalité positive, celle qui voit toujours les choses du bon côté. Elle privilégie ce qui respecte au plus votre paix intérieur et vous crée un maximum de bonheur.",
                Parties.Shyness => "C’est votre personnalité naturelle, sa priorité vos préférences. Elle privilégie ce qui vous tient le plus à cœur.\n",
                Parties.AI => "Cette personnalité est externe, c’est la partie de vous qui utilise l’IA pour répondre à tous les problème",
                Parties.Selfless => "C’est votre personnalité neutre, elle fait abstraction de tous les conflits. Elle privilégie la neutralité et non-réaction.",
                Parties.Anger => "C’est votre personnalité explosive, elle fait abstraction de tous les conflits. Elle privilégie la défense de vos intérêts.",
                _ => throw new ArgumentOutOfRangeException(nameof(party), party, null)
            };
        }
        
        public static string PartyName(this Parties party)
        {
            return party switch
            {
                Parties.Optimism => "Parti Optimiste",
                Parties.Shyness => "Parti Centré sur soi-même.",
                Parties.AI => "Parti Intelligence Artificielle",
                Parties.Selfless => "Parti M'en foutiste",
                Parties.Anger => "Parti Colère",
                _ => throw new ArgumentOutOfRangeException(nameof(party), party, null)
            };
        }
    }
}
