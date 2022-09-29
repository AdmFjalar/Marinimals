using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer;

    public LayerMask targets;
    public GameObject explosionEffect; //The prefab for the explosion effect on impact when shooting.

    public Sprite rangeCircle;
    public SpriteRenderer renderer;

    [SerializeField] private GameObject torpedoPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject carpetBombPrefab;
    [SerializeField] private GameObject incomingPrefab;

    public enum WeaponType { gun, mortar, torpedo, missile, carpetBomb, ram };
    public WeaponType type = WeaponType.mortar;

    public float delay = 2.5f;
    public float mortarSoundLength;
    public int damage;
    public int damageRange;
    public int range = 10;
    public float targetRange;

    public AudioClip travellingSound;
    public AudioClip shotSound;
    public AudioSource mainSource;

    private void Start()
    {
        targets = transform.parent.parent.parent.GetComponent<Player>().targetLayers;
        //timer = delay;
    }

    private void Update()
    {
        if (timer < delay)
        {
            timer += Time.deltaTime;
        }
    }

    //private void LateUpdate()
    //{
    //    if (renderer == null)
    //    {
    //        obj = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
    //        renderer = obj.AddComponent<SpriteRenderer>();
    //        renderer.sortingOrder = 1;
    //        renderer.sprite = rangeCircle;
    //    }
    //    obj.transform.localScale = new Vector2(range, range);
    //}

    public void Shoot()
    {
        if (timer >= delay)
        {
            mainSource.clip = shotSound;
            mainSource.PlayOneShot(shotSound);
            targetRange = Vector2.Distance(transform.parent.parent.GetComponentInChildren<Sight>().transform.GetChild(0).transform.position, transform.position);
            //targetRange = Mathf.Clamp(targetRange, 0, range);

            if (type == WeaponType.mortar)
            {
                Vector2 pos = transform.position + transform.up * targetRange;
                pos.x = pos.x + Random.Range(-1.5f, 1.5f);
                pos.y = pos.y + Random.Range(-1.5f, 1.5f);

                GameObject obj = Instantiate(incomingPrefab, pos, Quaternion.Euler(0, 0, 0));
                obj.GetComponent<Mortar>().SelfDestruct(delay);

                StartCoroutine(ShootMortar(mortarSoundLength, pos, obj));
            }
            else if (type == WeaponType.gun)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, targetRange, targets, -Mathf.Infinity, Mathf.Infinity);
                if (hit)
                {
                    Instantiate(explosionEffect, hit.point, Quaternion.identity);

                    if (hit.transform.GetComponent<Stats>() != null)
                    {
                        hit.transform.GetComponent<Stats>().TakeDamage(Random.Range(damage - damageRange, damage + damageRange) * GameManager.instance.damageModifier[transform.parent.gameObject.layer - 8]);
                    }
                }
                else if (!hit)
                {
                    Instantiate(explosionEffect, transform.position + transform.up * targetRange, Quaternion.identity);
                }
            }
            else if (type == WeaponType.torpedo)
            {
                GameObject torpedo = Instantiate(torpedoPrefab, transform.position + transform.up * 3, transform.rotation);
                torpedo.layer = transform.parent.gameObject.layer;
            }
            else if (type == WeaponType.missile)
            {
                GameObject missile = Instantiate(missilePrefab, transform.position + transform.up * 3, transform.rotation);
                missile.layer = transform.parent.gameObject.layer;
            }
            else if (type == WeaponType.carpetBomb)
            {
                GameObject carpetBomber = Instantiate(carpetBombPrefab);
                carpetBomber.layer = transform.parent.gameObject.layer;
                carpetBomber.GetComponent<CarpetBomber>().InitiateBombing(transform.position + (transform.up * targetRange));
            }
            else if (type == WeaponType.ram)
            {
                transform.parent.parent.GetComponent<ShipControl>().Charge();
            }

            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shoot");
            timer = 0;
        }
    }

    public IEnumerator ShootMortar(float delay, Vector2 pos, GameObject incoming)
    {

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = GameManager.instance.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup;
        source.clip = travellingSound;
        if (!source.isPlaying)
        {
            source.volume = 0.1f;
            source.PlayOneShot(travellingSound);
        }

        yield return new WaitForSeconds(delay);

        Instantiate(explosionEffect, pos, Quaternion.identity);

        Collider2D[] intersecting = Physics2D.OverlapCircleAll(pos, 2.5f, targets);
        if (intersecting.Length == 0)
        {
            //code to run if nothing is intersecting as the length is 0
        }
        else
        {
            foreach (Collider2D col in intersecting)
            {
                if (col.transform.GetComponent<Stats>() != null)
                {
                    col.transform.GetComponent<Stats>()?.TakeDamage(damage * GameManager.instance.damageModifier[transform.parent.gameObject.layer - 8]);
                }

                if (col.transform.parent != null)
                {
                    col.transform.parent.GetComponent<Stats>()?.TakeDamage(damage * GameManager.instance.damageModifier[transform.parent.gameObject.layer - 8]);
                }
            }
        }

        if (incoming != null)
        {
            Destroy(incoming);
        }
        Destroy(source);
    }
}
