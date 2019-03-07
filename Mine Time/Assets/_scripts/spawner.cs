using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using EZCameraShake;

public class spawner : MonoBehaviour {

    public GameObject depthnum, goldnum, healthbar;
    public GameObject game, death;
    public GameObject finaldepthnum, finalgoldnum, finalscore;
    public GameObject filter;

    public GameObject trail;
    public GameObject rock;
    public GameObject gold;
    public GameObject player;

    public float trail_spawn_delay = 0.1f;
    public float rock_spawn_delay = 50;
    public float gold_spawn_delay = 50;

    [SerializeField]
    Vector2 rocksize = new Vector2(0.5f, 0.8f);

    private float depth, scoregold;
    private int health = 3;

    private Queue<GameObject> trails = new Queue<GameObject>();
    private Queue<GameObject> rocks = new Queue<GameObject>();
    private Queue<GameObject> golds = new Queue<GameObject>();

    private float trailtimer = 0, rocktimer = 0, goldtimer = 0;
    private Vector3 filterscale1 = new Vector3(10f, 10f, 2f);
    private Vector3 filterscale2 = new Vector3(2f, 3f, 1f);
    private Vector3 filterscale3 = new Vector3(1.125f, 2f, 1f);

    private bool spawning = true;
    private player_controller plyr; 

    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Use this for initialization
    void Start()
    {
        plyr = player.GetComponent<player_controller>();
        EZCameraShake.CameraShaker.Instance.StartShake(0.15f, 1.5f, 0);
    }

	
	// Update is called once per frame
	void FixedUpdate () {
        if (spawning)
        {
            spawn_trail();
            spawn_rock();
            spawn_gold();
        }
        depth += plyr.ymove;
        depthnum.GetComponent<Text>().text = String.Format("{0:f}", depth);
        goldnum.GetComponent<Text>().text = scoregold.ToString();

        plyr.fall_speed += plyr.ymove * 0.025f;
        plyr.turn_speed += plyr.ymove * 0.005f;
        if (rock_spawn_delay >= 2)
            rock_spawn_delay -= plyr.ymove * 0.005f;
        gold_spawn_delay -= plyr.ymove * 0.01f;
        if (rocksize.x <= .8)
            rocksize += new Vector2(plyr.ymove, plyr.ymove) * 0.001f;
        if (filter.transform.localScale.x >= 1f)
            filter.transform.localScale += new Vector3(-0.01f, -0.01f, 0.0f) * plyr.ymove;
    }

    public void colliderock (GameObject rock)
    {
        rock.GetComponent<SpriteRenderer>().enabled = false;
        rock.GetComponent<PolygonCollider2D>().enabled = false;
        rock.GetComponent<ParticleSystem>().Play();
        rock.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.7f, 1.3f);
        rock.GetComponent<AudioSource>().Play();
        EZCameraShake.CameraShaker.Instance.ShakeOnce(5f, 2f, 0f, .5f);

        health--;
        if (health == 2)
            healthbar.GetComponent<Text>().text = "▲▲";
        if (health == 1)
            healthbar.GetComponent<Text>().text = "▲";
        if (health == 0)
        {
            healthbar.GetComponent<Text>().text = "";
            game.SetActive(false);
            death.SetActive(true);
            GetComponent<AudioSource>().Stop();
            player.GetComponent<SpriteRenderer>().enabled = false;
            finaldepthnum.GetComponent<Text>().text = String.Format("{0:f}", depth);
            finalgoldnum.GetComponent<Text>().text = scoregold.ToString();
            finalscore.GetComponent<Text>().text = (depth + scoregold).ToString();
            Time.timeScale = 0;
        }
    }

    public void collidegold (GameObject gold)
    {
        gold.GetComponent<SpriteRenderer>().enabled = false;
        gold.GetComponent<PolygonCollider2D>().enabled = false;
        gold.GetComponent<ParticleSystem>().Play();
        gold.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.7f, 1.3f);
        gold.GetComponent<AudioSource>().Play();
        scoregold += 10;
        EZCameraShake.CameraShaker.Instance.ShakeOnce(5f, 2f, 0f, .5f);
    }

    void spawn_gold()
    {
        float ymove = plyr.ymove;
        goldtimer += ymove;
        if (goldtimer >= gold_spawn_delay)
        {
            goldtimer = UnityEngine.Random.Range(-(gold_spawn_delay * .5f), (gold_spawn_delay * .5f)) ;
            Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-3f, 3f), -7f, -1.5f);
            Quaternion newrot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            GameObject newgold = Instantiate(gold, spawnpoint, newrot);
            golds.Enqueue(newgold);
        }
        foreach (GameObject g in golds)
            g.transform.position += new Vector3(0, ymove, 0);

        while (golds.Count > 0 && golds.Peek().transform.position.y >= 6)
        {
            Destroy(golds.Peek());
            golds.Dequeue();
        }
    }

    void spawn_rock()
    {
        float ymove = plyr.ymove;
        rocktimer += ymove;
        if (rocktimer >= rock_spawn_delay)
        {
            rocktimer = UnityEngine.Random.Range(-(rock_spawn_delay * .5f), (rock_spawn_delay * .5f)) ;
            Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-3f, 3f), -7f, -2);
            Quaternion newrot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            GameObject newrock = Instantiate(rock, spawnpoint, newrot);
            newrock.transform.localScale = new Vector3(UnityEngine.Random.Range(rocksize.x, rocksize.y), UnityEngine.Random.Range(rocksize.x, rocksize.y), 1);
            rocks.Enqueue(newrock);
        }
        foreach (GameObject r in rocks)
            r.transform.position += new Vector3(0, ymove, 0);

        while (rocks.Count > 0 && rocks.Peek().transform.position.y >= 6)
        {
            Destroy(rocks.Peek());
            rocks.Dequeue();
        }
    }

    void spawn_trail()
    {
        float ymove = plyr.ymove;
        trailtimer += ymove;
        trailtimer += Math.Abs(plyr.xmove);
        if (trailtimer >= trail_spawn_delay)
        {
            trailtimer = 0;
            GameObject newtrail = Instantiate(trail, player.GetComponent<Transform>().position, player.GetComponent<Transform>().rotation);
            Vector3 movetrail = new Vector3(0, 0, 1);
            //movetrail = player.GetComponent<Transform>().rotation * movetrail;
            newtrail.transform.position += movetrail;
            trails.Enqueue(newtrail);
        }

        foreach (GameObject t in trails)
            t.transform.position += new Vector3(0, ymove, 0);

        while (trails.Count > 0 && trails.Peek().transform.position.y >= 6)
        {
            Destroy(trails.Peek());
            trails.Dequeue();
        }
    }
}
