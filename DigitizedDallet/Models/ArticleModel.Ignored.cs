﻿using System.Reflection;

namespace DigitizedDallet.Models;

public partial class ArticleModel
{
    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public IEnumerable<ArticleModel> AllForms => AlternativeForms
        .Concat(SubArticles)
        .Concat(PluralForms)
        .Concat(FemininePluralForms)
        .Concat(FeminineForms)
        .Concat(SingularForms)
        .Concat(VerbalNouns)
        .Concat(Conjugations.SelectMany(x=> x.AllForms));

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public string? PartOfSpeech
    {
        get
        {
            if (Nature != null)
            {
                return Nature;
            }

            if (Conjugations.Any())
            {
                return "verb.";
            }

            if (PluralForms.Any()
                || FeminineForms.Any()
                || FemininePluralForms.Any()
                || SingularForms.Any())
            {
                return "noun.";
            }

            return Info;
        }
    }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public string? FirstTranslation
    {
        get
        {
            if (Meanings.FirstOrDefault()?.SameAs != null)
            {
                return Meanings.First().SameAs!.FirstTranslation;
            }

            if (AlternativeFormOf != null)
            {
                return this.AlternativeFormOf.FirstTranslation;
            }

            var f = Meanings.FirstOrDefault()?.Translations.FirstOrDefault();

            if (f == "#NULL")
            {
                return this.Info;
            }

            return f;
        }
    }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public RootModel Root { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel Resolved => RedirectTo ?? this;

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? RedirectTo { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public bool IsRedirected => RedirectTo != null;

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? AlternativeFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? SubArticleOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PluralFormOf { get; set; }


    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? FemininePluralFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? FeminineFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? SingularFormOf { get; set; }
    

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? VerbalNounOf { get; set; }

    /* region: conjugation */
    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? IntensiveThirdSingularFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? IntensiveThirdPluralFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? IntensiveThirdSingularFeminineFormOf { get; set; }
    
    
    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? ImperativePluralFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? ImperativePluralFeminineFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? IntensiveImperativeFormOf { get; set; }


    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PreteriteThirdPluralFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PreteriteThirdSingularFeminineFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PreteriteThirdSingularFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PreteriteFirstSingularFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? NegativePreteriteThirdSingularFormOf { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? NegativePreteriteThirdPluralFormOf { get; set; }
    /* endregion */

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    // DerivedTermOf is missing
    public ArticleModel? FormOf => AlternativeFormOf ?? PluralFormOf ?? FemininePluralFormOf ?? FeminineFormOf ?? SingularFormOf ?? VerbalNounOf;


    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public Dictionary<string, ArticleModel> FormOfProperties => GetType()
     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(x=> x.Name != nameof(FormOf) && x.Name.EndsWith("Of") && x.GetValue(this, null) is not null)
          .ToDictionary(x => x.Name, x => (ArticleModel)x.GetValue(this, null)!);


    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PrefixedArticle { get; set; }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    private int IndexInDoc => Root.Letter.Doc.MainArticles.IndexOf(this);

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? NextEntry
    {
        get
        {
            if (Root == null || FormOf != null)
                return null;

            int count = Root.Letter.Doc.MainArticles.Count;

            if (IndexInDoc >= count - 1)
            {
                return null;
            }

            return Root.Letter.Doc.MainArticles[IndexInDoc + 1];
        }
    }

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public ArticleModel? PreviousEntry
    {
        get
        {
            if (Root == null || FormOf != null)
                return null;

            if (IndexInDoc < 1)
            {
                return null;
            }

            return Root.Letter.Doc.MainArticles[IndexInDoc - 1];
        }
    }


    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public IEnumerable<MeaningModel> AllMeanings => Meanings.Concat(Meanings.SelectMany(x => x.AllMeanings));

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public IEnumerable<ExampleModel> AllExamples => Meanings.SelectMany(x => x.AllExamples);

    [System.Text.Json.Serialization.JsonIgnore][Newtonsoft.Json.JsonIgnore]
    public int Duplicates { get; set; }
}
