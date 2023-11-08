from flask import Flask, request, jsonify
import sqlite3 
import datetime
app = Flask(__name__)


def trytoLoginDatabase(email,password):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    res = cur.execute("SELECT ID FROM User WHERE email='"+email+"' and password='"+password+"'")
    if(res is None):
        return -1
    else:
        return res.fetchone()
def saveEdgeToDatabase(edge):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    
    cur.executemany("INSERT INTO Edge (MazeID,Cell1,Cell2) VALUES (?,?,?)",edge)
    con.commit()

def saveMazetoDatabase(userid,type):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    now = datetime.datetime.now()

    formatted_datetime = now.strftime("%Y-%m-%dT%H:%M:%S")
    cur.execute("INSERT INTO Maze (UserID,Type,CreationDate) VALUES (?,?,?)",(userid,type,formatted_datetime))
    con.commit()
    last_id = cur.lastrowid
    return last_id
def getMaze(id):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    res = cur.execute("SELECT * FROM Edge WHERE MazeID="+str(id))
    
    if(res is None):
        return []
    else:
        return res.fetchall()
def getMazeList(id):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    res = cur.execute("SELECT * FROM Maze WHERE UserID="+str(id))
    if(res is None):
        return []
    else:
        return res.fetchall()
def getMazeCount(id):
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    cur.execute("SELECT Count(*) FROM Maze WHERE UserID="+str(id))
    count = cur.fetchone()[0]
    print(count)
    con.close()
    return count
def countMazes() ->int:
    con = sqlite3.connect("test.db")
    cur = con.cursor()
    cur.execute("SELECT COUNT(*) FROM Maze")
    count = cur.fetchone()[0]
    print(count)
    con.close()
    return count
    
@app.route('/saveMaze', methods=['POST'])
def saveMaze():
    data = request.get_json()
    id = data.get('userID')
    edges = data.get('edges')
    mazeid=saveMazetoDatabase(id,"Classic")
    print(edges[0]['Cell1'])
    upredges=[]
    for edge in edges:
        tupled=(mazeid,edge['Cell1'],edge['Cell2'])
        upredges.append((tupled))
    print(upredges)
    saveEdgeToDatabase(upredges)
    response = {'message': id}
    status_code = 200
    return jsonify(response), status_code
@app.route('/loadMaze', methods=['POST'])
def loadMaze():
    data = request.get_json()
    id = data.get('mazeID')
    result=getMaze(id)
    response =  {'message': result}
    status_code = 200
    return jsonify(response), status_code
@app.route('/loadMazeCount', methods=['POST'])
def loadMazeCount():
    data = request.get_json()
    id = data.get('userID')
    result=getMazeCount(id)
    response =  {'message': result}
    status_code = 200
    return jsonify(response), status_code

@app.route('/loadMazeList', methods=['POST'])
def loadMazeList():
    data = request.get_json()
    id = data.get('userID')
    res=getMazeList(id)
    response =  {'descriptions': res}
    status_code = 200
    return jsonify(response), status_code

@app.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    email = data.get('email')
    password = data.get('password')
    print(email)
    print(password)
    id=trytoLoginDatabase(email,password)[0]

    if id>0:
        response = {'message': id}
        status_code = 200
    else:
        response = {'message': -1}
        status_code = 401

    return jsonify(response), status_code

if __name__ == '__main__':
    
    app.run(host='0.0.0.0', port=8085)
