import torch.nn as nn

class ConvolutionalNeuralNetwork(nn.Module):
    '''
    def __init__(self):
        super(ConvolutionalNeuralNetwork, self).__init__();

        self.conv1 = nn.Conv1d(17, 16, kernel_size=15, stride=1, padding=9);

        self.conv2 = nn.Conv1d(16, 32, kernel_size=15, stride=1, padding=9);

        self.conv3 = nn.Conv1d(32, 48, kernel_size=15, stride=1, padding=9);

        self.conv4 = nn.Conv1d(48, 64, kernel_size=15, stride=1, padding=9);

        self.conv5 = nn.Conv1d(64, 80, kernel_size=15, stride=1, padding=9);

        self.conv6 = nn.Conv1d(80, 96, kernel_size=15, stride=1, padding=9);

        self.line1 = nn.Linear(2400, 1200);

        self.line2 = nn.Linear(1200, 3);

    def forward(self, input, batchsize):
        tanh = nn.Tanh();
        relu = nn.ReLU();
        sigmoid = nn.Sigmoid();
        softMax = nn.Softmax(dim= 2);

        #print(input.size());
        x = self.conv1(input);
        x = relu(x);
        #print(x.size());

        x = self.conv2(x);
        x = relu(x);
        #print(x.size());

        x = self.conv3(x);
        x = relu(x);
        #print(x.size());

        x = self.conv4(x);
        x = relu(x);
        # print(x.size());

        x = self.conv5(x);
        x = relu(x);
        # print(x.size());

        x = self.conv6(x);
        x = relu(x);
        # print(x.size());

        x = x.view(batchsize, -1, 96*25);
        #print(x.size());

        x = self.line1(x);
        x = tanh(x);

        x = self.line2(x)

        return x;
    '''
    def __init__(self, input_size, output_size):
        super(ConvolutionalNeuralNetwork, self).__init__()

        self.conv1 = nn.Conv1d(input_size, int((input_size + 1) * 2), 7, padding=3, stride=1)
        self.conv2 = nn.Conv1d(int((input_size + 1) * 2), int((input_size + 1) * 4), 4, padding=2, stride=1)
        self.conv3 = nn.Conv1d(int((input_size + 1) * 4), int((input_size + 1) * 8), 4, padding=2, stride=1)
        self.conv4 = nn.Conv1d(int((input_size + 1) * 8), int((input_size + 1) * 16), 4, padding=2, stride=1)

        self.lin1 = nn.Linear(576, int((576) * ((2 / 3) ** 3)))
        self.lin2 = nn.Linear(int((576) * ((2 / 3) ** 3)), int((576) * ((2 / 3) ** 6)))
        self.lin3 = nn.Linear(int((576) * ((2 / 3) ** 6)), output_size)

        self.random_relu = nn.RReLU()
        self.softmax = nn.Softmax(dim=1)


    def forward(self, instances):
        pass
        # Convolutional:

        x = self.conv1(instances)
        x = self.random_relu(x)
        x = self.conv2(x)
        x = self.random_relu(x)

        x = self.conv3(x)
        x = self.random_relu(x)
        x = self.conv4(x)
        x = self.random_relu(x)

        # Linear:
        x = x.view(-1, 576) # 576
        x = self.lin1(x)
        x = self.random_relu(x)
        x = self.lin2(x)
        x = self.random_relu(x)
        x = self.lin3(x)
        x = self.softmax(x)
        return x
