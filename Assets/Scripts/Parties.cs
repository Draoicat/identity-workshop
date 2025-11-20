using System;

public enum Parties : int
{
    Optimism = 0,
    Shyness = 1,
    AI = 2,
    Selfless = 3,
    Anger = 4
}

static class PartiesExtensions 
{
  public static string PartyDescription(this Parties party)
  {
      return party switch
      {
          Parties.Optimism => "C'est le parti optimiste.",
          Parties.Shyness => "C'est la parti timide.",
          Parties.AI => "C'est le parti LLM",
          Parties.Selfless => "C'est le parti j'menfoutiste",
          Parties.Anger => "C'est le parti colérique",
          _ => throw new ArgumentOutOfRangeException(nameof(party), party, null)
      };
  }
}
