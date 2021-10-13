
using UnityEngine;

public class wallMaker : MonoBehaviour
{

    Transform player;
    public GameObject blockPrefab;

    //sürekli son bloğu alıp ona göre ekleme yapıcaz o yüzden son blok referans olarak lazım
    public Transform lastBlock;
    Vector3 lastBlockPos;
    private float offset= 0.70711f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = FindObjectOfType<PlayerController>().transform;
        lastBlockPos = lastBlock.position;

        //her saniyede 8 tane block oluşturcak
        InvokeRepeating("CreateWall", 0f, 1f/2f);
        
    }

    // Update is called once per frame
    void Update()
    {
        //CreateWall();
    }

    private void CreateWall()
    {

        //cameranın bakış açısına göre wall oluşturmayı sağladık
        float distance = Vector3.Distance(player.position, lastBlockPos);
        if (distance > cam.orthographicSize*2)
        {
            return;
        }



        var newBlock = Instantiate(blockPrefab, transform);
        int chance = Random.Range(1, 11);

        //1-10 arası random sayı üretiyor eğer sayı 5in üstündeyse block başka tarafa 5,n üstünde değilse başka alana yapılacak
        if (chance > 5)
        {
            newBlock.transform.position = new Vector3(lastBlockPos.x + offset, lastBlockPos.y, lastBlockPos.z + offset );
        }
        else
        {
            newBlock.transform.position = new Vector3(lastBlockPos.x - offset, lastBlockPos.y, lastBlockPos.z + offset );
        }

        //x'in yeni konumu tanımlanan offset değerinin toplanmasıyla oluşan yeni değer
        //y'yi değiştirmiyoruz, eski konumunda kalabilir.
        //newBlock.transform.position = new Vector3(lastBlockPos.x + offset, lastBlockPos.y, lastBlockPos.z + offset );
        
        //her block 45 derecelik açıya sahip
        newBlock.transform.rotation = Quaternion.Euler(0, 45, 0);
        lastBlockPos = newBlock.transform.position;


        //crystal oluşturma chance değerinin 4e bölümünden kalan 1 olduğunda crystali enable yap
        bool randomCrystal = chance % 3 == 2;
        newBlock.transform.GetChild(0).gameObject.SetActive(randomCrystal);
    
    }


}
