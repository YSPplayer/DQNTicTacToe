import sys
import json
import numpy as np
import tensorflow as tf
def mode_get_s_value(s,av_s):
    # 测试模型
    #model = tf.keras.models.load_model('../Model/dqn_model.h5')
    model = tf.keras.models.load_model('D:/ZfraTemp/DQNTicTacToe/bin/Debug/Model/dqn_model.h5')
    # 变形
    state_batch = np.expand_dims(s, axis=0)
    # 进行预测
    predictions = model.predict(state_batch)
    max_value = float('-inf')
    # 变成6*6的矩阵
    actions = np.reshape(predictions[0], (6, 6))
    max_index = None
    for index in av_s:
        value = s[index[0]][index[1]]
        if value > max_value:
            max_value = value
            max_index = index
    return max_index
if __name__ == "__main__":
    #matrix_json = sys.argv[1] #获取输入
    #list_json = sys.argv[2]
    matrix_json = '[[0,1,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0],[0,0,0,0,0,0]]'
    list_json = '[[0,0],[0,2],[0,3],[0,4],[0,5],[1,0],[1,1],[1,2],[1,3],[1,4],[1,5],[2,0],[2,1],[2,2],[2,3],[2,4],[2,5],[3,0],[3,1],[3,2],[3,3],[3,4],[3,5],[4,0],[4,1],[4,2],[4,3],[4,4],[4,5],[5,0],[5,1],[5,2],[5,3],[5,4],[5,5]]'

    # 将 JSON 字符串转换为 Python 对象
    s = np.array(json.loads(matrix_json))
    av_s = [np.array(item) for item in json.loads(list_json)]
    #max_index = mode_get_s_value(s,av_s)
    #result = str(max_index[0]) + "," + str(max_index[1])
    #print(result)
