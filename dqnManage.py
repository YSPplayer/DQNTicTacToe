import tensorflow as tf
from collections import deque
import numpy as np
import random

class DQNManage:
    @staticmethod
    def test_mode(s1,action_count):
        #测试模型
        model = tf.keras.models.load_model('dqn_model.h5')
        #变形
        state_batch = np.expand_dims(s1, axis=0)
        # 进行预测
        predictions = model.predict(state_batch)
        predicted_value = predictions[0]
        #打印所有动作的预测值
        print("Predicted value for actions is ", predicted_value)
        # 打印每个动作及其对应的预测值
        for action_index, value in enumerate(predictions[0]):
            print("Action:", action_index, "Predicted value:", value)
        print("the count for action is", action_count)
    @staticmethod
    def build_dqn(state_shape, action_size):
        #构建神经网络模型，包含全连接层和卷积层，最终输出每一个动作的Q值
        model = tf.keras.models.Sequential([
            tf.keras.layers.Conv2D(32, (3, 3), activation='relu', input_shape=state_shape),
            tf.keras.layers.Conv2D(64, (3, 3), activation='relu'),
            tf.keras.layers.Flatten(),
            tf.keras.layers.Dense(64, activation='relu'),
            tf.keras.layers.Dense(action_size, activation='linear')
        ])
        #0.001是学习率
        model.compile(optimizer=tf.keras.optimizers.Adam(learning_rate=0.001), loss='mse')
        return model

    @staticmethod
    def train_dqn(model, replay_buffer, batch_size, epochs, epsilon_start=1.0, epsilon_end=0.01, epsilon_decay=0.995):
        epsilon = epsilon_start
        for epoch in range(epochs):
            minibatch = replay_buffer.sample(batch_size)
            for state, action, reward, next_state, action_size, done in minibatch:
                state_batch = np.expand_dims(state, axis=0)
                # ε-greedy 策略
                if np.random.rand() <= epsilon:
                    # 探索：随机选择动作
                    selected_action = np.random.randint(0, action_size)
                else:
                    # 利用：选择最佳预测动作
                    selected_action = np.argmax(model.predict(state_batch)[0])

                # 更新 epsilon
                epsilon = max(epsilon_end, epsilon * epsilon_decay)
                # 计算目标值
                target = reward
                if not done:
                    # 将形状从 (6, 6, 1) 改为 (1, 6, 6, 1) 表示批量的模型，这里我们只有1个
                    next_state_batch = np.expand_dims(next_state, axis=0)
                    target = reward + 0.95 * np.amax(model.predict(next_state_batch)[0])
                target_f = model.predict(state_batch)
                target_f[0][selected_action] = target
                # 单步训练
                model.fit(state_batch, target_f, epochs=1, verbose=0)
    @staticmethod
    def get_dqn_model_message(model_path):
        dqn_model = tf.keras.models.load_model(model_path)
        dqn_model.summary()
    @staticmethod
    def continue_training(samples,model_path,epochs):
        s1_data = np.array([sample.s1 for sample in samples])
        action_data = np.array([sample.action for sample in samples])
        value_data = np.array([sample.value for sample in samples])
        action_count_data = np.array([sample.action_count for sample in samples])
        end_data = np.array([sample.end for sample in samples])
        s1_data = np.expand_dims(s1_data, axis=-1)  # 这里要对s1状态增加一个新的维度
        # 经验池中添加样本
        replay_buffer = ReplayBuffer()
        for idx in range(len(s1_data) - 1):
            replay_buffer.add(s1_data[idx], action_data[idx], value_data[idx], s1_data[idx + 1],
                              action_count_data[idx], end_data[idx])
        # 加载已有模型
        dqn_model = tf.keras.models.load_model(model_path)
        # 继续训练
        DQNManage.train_dqn(dqn_model, replay_buffer, batch_size=32, epochs=epochs)
        # 保存模型
        dqn_model.save(model_path)

    @staticmethod
    def start_train(samples,model_path,epochs):
        s1_data = np.array([sample.s1 for sample in samples])
        action_data = np.array([sample.action for sample in samples])
        value_data = np.array([sample.value for sample in samples])
        action_count_data = np.array([sample.action_count for sample in samples])
        end_data = np.array([sample.end for sample in samples])
        s1_data = np.expand_dims(s1_data, axis=-1) #这里要对s1状态增加一个新的维度
        #经验池中添加样本
        replay_buffer = ReplayBuffer()
        for idx in range(len(s1_data) - 1):
            replay_buffer.add(s1_data[idx], action_data[idx], value_data[idx], s1_data[idx + 1],
                              action_count_data[idx],end_data[idx])
        #开始训练
        # 构建和训练模型
        state_shape = (6, 6, 1)  # 6x6的矩阵，单一颜色通道
        action_size = 36  # 假设最多有36种可能的动作
        dqn_model = DQNManage.build_dqn(state_shape, action_size)
        #epochs 训练次数 batch_size 是指在训练过程中一次性处理的数据样本数
        DQNManage.train_dqn(dqn_model, replay_buffer, batch_size=32, epochs=epochs)
        # 保存模型
        dqn_model.save(model_path)
#ReplayBuffer 类：用于存储和重放以前的经验（状态、动作、奖励等）。
#这对于打破样本之间的相关性和避免过拟合至关重要。
class ReplayBuffer:
    def __init__(self, capacity=100000):
        self.buffer = deque(maxlen=capacity)

    def add(self, state, action, reward, next_state, action_size, done):
        #done表示当前游戏是否停止，我们要对游戏的一个连续的序列环境进行经验池的使用
        self.buffer.append((state, action, reward, next_state, action_size, done))

    def sample(self, batch_size):
        return random.sample(self.buffer, batch_size)