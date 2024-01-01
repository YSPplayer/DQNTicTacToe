import sqlite3
import numpy as np

#Sample类
class Sample:
    def __init__(self, s1, action, value, action_count, end):
        self.s1 = DataManage.string_to_numpy_array(s1)
        self.action = DataManage.string_to_numpy_array(action,True)
        self.value = value
        self.action_count = action_count
        bend = False
        if end == 1:
            bend = True
        else:
            bend = False
        self.end = bend
class DataManage:
    @staticmethod
    def string_to_numpy_array(str,is_action=False):
        if is_action:
            #一维矩阵
            return np.array([int(item) for item in str.split(',')])
        else:
            # 将字符串转换成整数列表
            elements = [int(item) for item in str.split(',')]
            # 确保元素数量是 36（6x6）
            if len(elements) != 36:
                return None
            # 转换为 6x6 的 NumPy 数组
            return np.array(elements).reshape(6, 6)
    @staticmethod
    def search_data():
        # 连接数据库
        conn = sqlite3.connect('./SampleData/sample.db')
        # 查询并读取数据
        samples = []
        cursor = conn.cursor()
        query = 'SELECT * FROM sampleTable'
        cursor.execute(query)
        for row in cursor.fetchall():
            #0位置是id索引
            new_sample = Sample(row[1], row[2], row[3], row[4], row[5])
            # 存储我们的数据
            samples.append(new_sample)
        # 关闭数据库连接
        conn.close()
        return samples

