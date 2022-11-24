using System;
using System.Collections;

namespace projet
{
    abstract class Compte
    {
        private int numero;
        private static int nombreDeCompte = 1;
        private Personne titulaireCompte;
        private List<OperationBancaire> operationBancaires;


        public Compte(Personne titulaireCompte)
        {
            numero = nombreDeCompte;
            this.titulaireCompte = titulaireCompte;
            nombreDeCompte++;
        }


        public int Numero
        {
            get { return numero; }
        }

        public Personne TitulaireCompte
        {
            get { return titulaireCompte; }
        }

        public List<OperationBancaire> OperationBancaires
        {
            get { return operationBancaires; }
        }

        public abstract void Crediter(double credit);

        public abstract void Debiter(double debit);

        public void Virement(Compte c1, Compte c2)
        {
            double result;
            double SaisieNombre()
            {
                while (!double.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            double credit = SaisieNombre();
            c1.Crediter(credit);
            double debit = SaisieNombre();
            c2.Debiter(debit);
            OperationBancaire o1 = new OperationBancaire(credit, "virement depuis le compte"+c2.Numero);
            c1.operationBancaires.Add(o1);
            OperationBancaire o2 = new OperationBancaire(credit, "virement sur le compte" + c1.Numero);
            c2.operationBancaires.Add(o2);
        }
    }

    class Courant:Compte{
        private double solde;
        private double decouvertMaximalAutorise;
        private double debitMaximalAutorise;

        public Courant(Personne titulaireCompte, double solde, double decouvertMaximalAutorise, double debitMaximalAutorise) : base(titulaireCompte)
        {
            if(solde < decouvertMaximalAutorise)
            {
                Console.WriteLine("Le solde ne peut pas être au dessus du découvert maximal autorisé !");
                this.solde = 0;
            }
            else
            {
                this.solde = solde;
            }
            this.decouvertMaximalAutorise = decouvertMaximalAutorise;
            this.debitMaximalAutorise = debitMaximalAutorise;
        }

        public override void Crediter(double credit)
        {
            solde += credit;
            OperationBancaire o = new OperationBancaire(credit, "compte crédité");
            OperationBancaires.Add(o);
        }

        public override void Debiter(double debit)
        {
            if(debit > debitMaximalAutorise)
            {
                Console.WriteLine("Le montant débité ne peut pas être superieur au débit maximal autorisé !");
            }
            else  if(solde - debit < decouvertMaximalAutorise)
            {
                Console.WriteLine("Le solde ne peut pas être inférieur au découvert maximal autorisé !");
            }
            else
            {
                solde -= debit;
                OperationBancaire o = new OperationBancaire(debit, "compte débité");
                OperationBancaires.Add(o);
            }
        }

    }

    class Epargne : Compte
    {
        private double solde;
        private double interet;

        public Epargne(Personne titulaireCompte, double solde, double interet) : base(titulaireCompte)
        {
            if(solde < 0)
            {
                Console.WriteLine("Le solde ne peut pas être négatif pour un compte d'épargne");
                this.solde = 0;
            }
            else
            {
                this.solde = solde;
            }
            this.interet = interet;
        }

        public override void Crediter(double credit)
        {
            solde += credit;
            OperationBancaire o = new OperationBancaire(credit, "compte crédité");
            OperationBancaires.Add(o);
        }

        public override void Debiter(double debit)
        { 
            if (solde - debit < 0)
            {
                Console.WriteLine("Le solde ne peut pas être inférieur à 0 !");
            }
            else
            {
                solde -= debit;
                OperationBancaire o = new OperationBancaire(debit, "compte débité");
                OperationBancaires.Add(o);
            }
        }


    }

    class Personne
    {
        private string nom;
        private int numeroCarteIdentite;
        private string adresse;

        public Personne(string nom, int numeroCarteIdentite, string adresse)
        {
            this.nom = nom;
            this.numeroCarteIdentite = numeroCarteIdentite;
            this.adresse = adresse;
        }


    }

    class OperationBancaire
    {
        private int numero;
        private int nombreDeOperation = 1;
        private double montant;
        private string date;
        private string libelle;

        public OperationBancaire(double montant, string libelle)
        {
            numero = nombreDeOperation;
            nombreDeOperation++;
            this.montant = montant;
            date = DateTime.Now.ToString();
            this.libelle = libelle;
        }
    }
}
