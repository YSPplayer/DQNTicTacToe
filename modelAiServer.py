import socket
import sys
import json
import numpy as np
from dropPiece import ModelAi
if __name__ == "__main__":
    try:
        path = sys.argv[1]
    except Exception as e:
        path = "D:\\ZfraTemp\\DQNTicTacToe\\bin\\Debug\\Model\\dqn_model.h5"

    # 创建 socket 对象
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # 获取本地主机名
    host = '127.0.0.1'
    port = 7913
    server_socket.bind((host, port))
    # 开始监听请求,操作流可以挂起的最大未完成连接的数量
    server_socket.listen(5)
    while True:
        ModelAi.init(path)  # 初始化模型
        print("Server OK")
        # 建立客户端连接
        client_socket, addr = server_socket.accept()
        client_ip = addr[0]
        if client_ip != "127.0.0.1":#非本机地址不连接
            client_socket.close()  # 关闭非本机的连接
            continue  # 继续等待下一个连接
        s = None
        av_s = None
        while True:
            data = client_socket.recv(1024)  # 读取 1024 字节的数据
            #ModelAi.mode_get_s_value()
            #client_socket.send(message.encode('ascii'))
            if data:
                message = data.decode('utf-8')
                if len(message) <= 0:
                    break
                if message == "PING":
                    print("PING")
                    continue
                elif message == "CLOSE":
                    print("CLOSE")
                    break
                elif message[0] == "A":
                    matrix_json = message[1:]
                    print(matrix_json)
                    s = np.array(json.loads(matrix_json))
                elif message[0] == "B":
                    list_json = message[1:]
                    print(list_json)
                    av_s = [np.array(item) for item in json.loads(list_json)]
                    max_index = ModelAi.mode_get_s_value(s,av_s)
                    result = str(max_index[0]) + "," + str(max_index[1])
                    print(result)
                    client_socket.send(result.encode('utf-8'))
                    s = None
                    av_s = None
        client_socket.close()
        break
    server_socket.close()
    print("Server CLOSE")