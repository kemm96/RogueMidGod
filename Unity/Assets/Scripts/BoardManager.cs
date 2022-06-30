using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public int columns = 8;
	public int rows = 8;

	public GameObject floor, wall,innerWall, heart, exit;
	public GameObject[] enemy;

	private Transform boardHolder;

	private List<Vector2> gridPositions = new List<Vector2>();

	void InitializeList(){
		gridPositions.Clear();
		for(int x=1; x<columns-1; x++){
			for(int y=1; y<rows-1; y++){
				gridPositions.Add(new Vector2(x, y));
			}
		}
	}

	Vector2 RandomPosition(){
		int randomIndex = Random.Range(0, gridPositions.Count);
		Vector2 position = gridPositions[randomIndex];
		gridPositions.RemoveAt(randomIndex);
		return position;
	}

	void LayoutObjectRandom(GameObject sprite, int min, int max){
		if(gridPositions.Count != 0){
			int objectCount = Random.Range(min, max+1);
			for(int i = 0; i<objectCount; i++){
				Vector2 position = RandomPosition();
				GameObject instance = Instantiate(sprite, position, Quaternion.identity);
				instance.transform.SetParent(boardHolder);
			}
		}
	}

	/* void LayoutExit(){
		Debug.Log(gridPositions[gridPositions.Count-1]);
		Vector2 exitPosition = gridPositions[gridPositions.Count-1];
		gridPositions.RemoveAt(gridPositions.Count-((columns*2)+1));
		gridPositions.RemoveAt(gridPositions.Count-(columns+1));
		gridPositions.RemoveAt(gridPositions.Count-1);
		gridPositions.RemoveAt(1);
		Instantiate(exit, exitPosition, Quaternion.identity);
	} */

	public void SetupScene(int level){
		int enemyUno = level / 2;
		int enemyDos = level - enemyUno;
		BoardSetup();
		InitializeList();
		/* LayoutExit(); */
		LayoutObjectRandom(heart,1,2);
		LayoutObjectRandom(innerWall,5,10);
		LayoutObjectRandom(enemy[0],enemyUno,enemyUno);
		LayoutObjectRandom(enemy[1],enemyDos,enemyDos);
	}

	void BoardSetup(){
		boardHolder = new GameObject("Board").transform;
		for(int x=-1; x<columns+1; x++){
			for(int y=-1; y<rows+1; y++){
				GameObject toInstantiate;
				if( x==-1 || y==-1 || x==columns || y==rows){
					toInstantiate = wall;
				}else{
					toInstantiate = floor;
				}

				GameObject instance = Instantiate(toInstantiate, new Vector2(x,y), Quaternion.identity);
				instance.transform.SetParent(boardHolder);
			}
		}
	}
}
