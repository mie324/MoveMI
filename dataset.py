import torch.utils.data as data


class signalDataset(data.Dataset):

    def __init__(self, features, labels):
        self.features = features;
        self.labels = labels;


    def __len__(self):
        return len(self.labels)

    def __getitem__(self, index):
        feat = self.features[index];
        label = self.labels[index];

        return feat, label;