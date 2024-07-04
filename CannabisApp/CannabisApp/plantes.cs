using DocumentFormat.OpenXml.Drawing.Diagrams;

public class plantes
{
    public int id_plante { get; set; }
   public string description { get; set; }
    public string Quentite { get; set; }
    public string code_qr { get; set; }
    public int id_provenance { get; set; }
    public int etat_sante { get; set; }
    public bool nombre_plantes_actives { get; set; }
    public DateTime date_expiration { get; set; }
    public DateTime cree_le { get; set; }
    public string stade { get; set; }
    public string Note { get; set; }
    public string identification { get; set; }
    public int id_Enterposage { get; set; }
}
