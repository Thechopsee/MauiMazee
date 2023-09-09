from flask import Flask, request, jsonify
import sqlite3 
app = Flask(__name__)

@app.route('/getGPSData', methods=['POST'])
def getGPSData():
    data = request.get_json()
    id = data.get('zSirka')
    print(id)
    response = {'message': 200}
    status_code = 200
    return jsonify(response), status_code

if __name__ == '__main__':
    
    app.run(host='10.0.0.43', port=1245)