using System;
using System.Collections;

namespace projet
{
    abstract class Compte
    {
        protected int numero;
        protected static int nombreDeCompte = 1;
        protected Personne titulaireCompte;
        protected List<OperationBancaire> operationBancaires;


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
            double virement = SaisieNombre();
            c1.Crediter(virement);
            c2.Debiter(virement);
            OperationBancaire o1 = new OperationBancaire(virement, "virement depuis le compte numéro "+c2.Numero);
            c1.operationBancaires.Add(o1);
            OperationBancaire o2 = new OperationBancaire(virement, "virement sur le compte numéro " + c1.Numero);
            c2.operationBancaires.Add(o2);
        }
    }

    class Courant:Compte{
        private double solde;
        private double decouvertMaximalAutorise;
        private double debitMaximalAutorise;
        private bool decouvert;
        private double debitAutorise;

        public Courant(Personne titulaireCompte, double solde, double decouvertMaximalAutorise, double debitMaximalAutorise) : base(titulaireCompte)
        {
            if(solde < decouvertMaximalAutorise)
            {
                Console.WriteLine("Le solde ne peut pas être en dessous du découvert maximal autorisé !");
                this.solde = 0;
            }
            else
            {
                if(solde < 0)
                {
                    decouvert = true;
                }
                else
                {
                    decouvert = false;
                }
                this.solde = solde;
            }
            this.decouvertMaximalAutorise = decouvertMaximalAutorise;
            this.debitMaximalAutorise = debitMaximalAutorise;
            if(debitMaximalAutorise <= solde)
            {
                debitAutorise = debitMaximalAutorise;
            }
            else
            {
                debitAutorise = debitAutorise - Math.Abs(solde);
            }
        }

        public bool Decouvert
        {
            get { return decouvert; }
        }

        public double DecouvertMaximalAutorise
        {
            get { return decouvertMaximalAutorise; }
            set { decouvertMaximalAutorise = value; }
        }

        public double DebitMaximalAutorise
        {
            get { return debitMaximalAutorise; }
            set { debitMaximalAutorise = value; }
        }

        public override void Crediter(double credit)
        {

            solde += credit;
            if (solde < 0)
            {
                decouvert = true;
            }
            else
            {
                decouvert = false;
            }
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
                if (solde < 0)
                {
                    decouvert = true;
                }
                else
                {
                    decouvert = false;
                }
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

        public string Nom
        {
            get { return nom; }
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

    class Banque
    {
        private string nom;
        private List<Compte> comptes;
        private List<Personne> clients;

        public Banque(string nom)
        {
            this.nom = nom;
        }

        public string Nom
        {
            get { return nom; }
        }

        public List<Compte> Comptes
        {
            get { return comptes; }
        }

        public List <Personne> Clients
        {
            get { return clients; }
        }

        public void CreerClient()
        {
            Console.WriteLine("Nom du client :");
            string nomClient = Console.ReadLine();
            Console.WriteLine("Adresse du client :");
            string adresseClient = Console.ReadLine();
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            int numeroCarteIdentiteClient = SaisieNombre();
            Personne client = new Personne(nomClient, numeroCarteIdentiteClient, adresseClient);
            clients.Add(client);
        }

        public void SupprimerClient()
        {
            Console.WriteLine("Nom du client à supprimer :");
            string nomClient = Console.ReadLine();
            foreach(Personne client in clients)
            {
                if(client.Nom == nomClient)
                {
                    clients.Remove(client);
                    return;
                }
            }
            Console.WriteLine("le nom renseigné ne correspond pas à un client de la banque !");
        }

        public void ajouterCompte()
        {
            Console.WriteLine("Compte épargne (0) ou compte courant (1) ?");
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            int typeCompte = SaisieNombre();
            if(typeCompte == 0)
            {
                Console.WriteLine("solde du compte : ");
                double resultDouble;
                 double SaisieNombreDouble()
                {
                    while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                    return resultDouble;
                }
                double soldeCompte = SaisieNombreDouble();
                Console.WriteLine("intéret du compte : ");
                double interet = SaisieNombreDouble();
                Console.WriteLine("nom du titulaire : ");
                string nomTitulaire = Console.ReadLine();
                bool verifTitulaire = false;
                foreach (Personne client in clients)
                {
                    if (client.Nom == nomTitulaire)
                    {
                        Personne p = client;
                        verifTitulaire = true;
                        Epargne e = new Epargne(p, soldeCompte, interet);
                        comptes.Add(e);
                    }
                }
                if (!verifTitulaire)
                {
                    Console.WriteLine("Le titulaire du compte ne fait pas parti des clients de la banque !");
                    return;
                }
            }
            else if(typeCompte == 1)
            {
                Console.WriteLine("solde du compte : ");
                double resultDouble;
                double SaisieNombreDouble()
                {
                    while (!double.TryParse(Console.ReadLine(), out resultDouble)) ;
                    return resultDouble;
                }
                double soldeCompte = SaisieNombreDouble();
                Console.WriteLine("découvert max du compte : ");
                double decouvertMaximalAutorise = SaisieNombreDouble();
                Console.WriteLine("débit maximal du compte : ");
                double debitMaximalAutorise = SaisieNombreDouble();
                Console.WriteLine("nom du titulaire : ");
                string nomTitulaire = Console.ReadLine();
                bool verifTitulaire = false;
                foreach (Personne client in clients)
                {
                    if (client.Nom == nomTitulaire)
                    {
                        Personne p = client;
                        verifTitulaire = true;
                        Courant c = new Courant(p, soldeCompte, decouvertMaximalAutorise, debitMaximalAutorise);
                        comptes.Add(c);
                    }
                }
                if (!verifTitulaire)
                {
                    Console.WriteLine("Le titulaire du compte ne fait pas parti des clients de la banque !");
                    return;
                }
            }
        }

        public void SupprimerCompte()
        {
            int result;
            int SaisieNombre()
            {
                while (!int.TryParse(Console.ReadLine(), out result)) ;
                return result;
            }
            Console.WriteLine("Saisir le numéro de compte à supprimer : ");
            int numero = SaisieNombre();
            foreach(Compte compte in comptes)
            {
                if(compte.Numero == numero)
                {
                    comptes.Remove(compte);
                    return;
                }
            }
            Console.WriteLine("Le numéro de compte renseigné n'est pas attribué à un compte existant !");
        }


        class TestProjet
        {
            static void Main(string[] args)
            {
                ConsoleKeyInfo cki;

                int result;

                int SaisieNombre()
                {
                    while (!int.TryParse(Console.ReadLine(), out result)) ;
                    return result;
                }
                Console.WriteLine("Saisir le nom de la banque:");
                string nomBanque = Console.ReadLine();
                Banque banque = new Banque(nomBanque);
                do
                {
                    Console.WriteLine("Bienvenu dans la banque " + banque.Nom);
                    Console.WriteLine("Vous êtes un client (0) ou un banquier (1) ?");
                    int typePersonne = SaisieNombre();
                    switch (typePersonne){
                        case 0:
                            Console.WriteLine("Veuillez saisir votre nom :");
                            string nomClient = Console.ReadLine();
                            foreach(Personne client in banque.Clients)
                            {
                                if(nomClient == client.Nom)
                                {
                                    Console.WriteLine("Bienvenu "+ client.Nom);
                                    Console.WriteLine("Menu client`:\n" +
                                                      "action 1: voir les infos de mes comptes"+
                                                      "action 2: créditer un de mes compte"+
                                                      "action 3: débiter un de mes comptes");
                                }
                            }
                            Console.WriteLine("Le client n'existe pas ! Veuillez contacter un banquier pour créer votre compte client.");
                            break;
                            
                        case 1:
                    }
                    Console.WriteLine("Tapez Escape pour sortir ou de l'exo");
                    cki = Console.ReadKey();
                } while (cki.Key != ConsoleKey.Spacebar);
            }
            ()
            }

    }
}
