<template>
    <div>
        <div class="tops">
            <el-tree :default-expanded-keys="['1']" ref="myTree" :data="data" :props="defaultProps" @node-click="handleNodeClick" highlight-current node-key="value">
                <span class="custom-tree-node" slot-scope="{ node, data }">
                    <i v-if="node.level == 1" class="father" />
                    <i v-else-if="node.level == 2" class="son" />
                    <i v-else-if="node.level == 3" />
                    {{ data.label }}
                </span>
            </el-tree>
        </div>
    </div>
</template>

<script>
    let that = this
    export default {
        data() {
            return {
                data: [
                    {
                        label: '1号生活区',
                        value: '1',
                        isShow: true,//当前折叠部分是否打开
                        children: [
                            {
                                label: '1号楼',
                                value: '1-1',
                            },
                            {
                                label: '2号楼',
                                value: '1-2',
                            },
                            {
                                label: '3号楼',
                                value: '1-3',
                            },
                            {
                                label: '4号楼',
                                value: '1-4',
                            },
                        ]
                    },
                    {
                        label: '2号生活区',
                        value: '2',
                        isShow: false,
                        children: [
                            {
                                label: '1号楼',
                                value: '2-1',
                            },
                            {
                                label: '2号楼',
                                value: '2-2'
                            },
                        ]
                    },
                    {
                        label: '3号生活区',
                        value: '3',
                        isShow: false,
                        children: [
                            {
                                label: '1号楼',
                                value: '3-1',
                            },
                            {
                                label: '2号楼',
                                value: '3-2',
                            },
                            {
                                label: '3号楼',
                                value: '3-3',
                            },
                        ]
                    },
                    {
                        label: '4号生活区',
                        value: '4',
                        isShow: false,
                        children: [
                            {
                                label: '1号楼',
                                value: '4-1',
                            },
                            {
                                label: '2号楼',
                                value: '4-2',
                            },
                            {
                                label: '3号楼',
                                value: '4-3',
                            },
                        ]
                    }
                ],
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
                value1: '',
                value2: '',

            };

        },
        methods: {
            handleNodeClick(data) {
                this.$emit('treesVal', data)
            },
        },
        mounted() {
            this.$nextTick(function () {
                this.$nextTick(() => {
                    // myTree 数组件的ref  默认让第一项高亮
                    // data是树组件对应的数据
                    // 通过data中的第一项的id找到对应的节点，然后把这个节点设置为高亮
                    this.$refs.myTree.setCurrentKey(this.data[0].value)
                });
            });
        },
    };
</script>

<style lang="scss" scoped>
   

    .tops {
        width: vw(260);
        height: 100%;
        background-color: #fff;
        margin: 0 auto;
        padding: 0 vw(8);
        box-shadow: 0 vw(2) vw(10) 0 rgba(0,5,10,0.1);
        //设置行高
        > > > .el-tree-node__content {
            position: relative;
            height: vh(52);
            box-sizing: border-box;
        }
        //小三角的位置 以及默认时候的位置
        > > > .el-tree-node__expand-icon {
            webkit-transform: rotate(-90deg);
            transform: rotate(-90deg);
            user-select: none;
            position: absolute;
            top: vh(8);
            right: 0;
        }
        //小三角点击展开时候旋转的角度
        > > > .el-tree-node__expand-icon.expanded {
            webkit-transform: rotate(90deg);
            transform: rotate(90deg);
            user-select: none;
        }
        //插入的父级的图标的样式
        .father {
            background: url('../assets/img/flower-wxz.png') no-repeat;
            width: vw(18);
            height: vw(18);
            display: inline-block;
            background-size: 100% 100%;
            // margin-left: vw(20) !important;
            margin: 0 vw(8) 0 vw(20) !important;
        }
        //插入的自己的图标样式
        .son {
            width: vw(4);
            height: vw(4);
            border-radius: 50%;
            background: #666666;
            display: inline-block;
            background-size: 100% 100%;
            margin: 0 vw(8) vh(3) vw(35) !important;
        }
        //有子节点 且未展开 小三角
        .el-tree /deep/ .el-icon-caret-right:before {
            color: #858585;
            font-weight: 600;
        }
        //有子节点 且已展开 小三角
        .el-tree /deep/ .el-tree-node__expand-icon.expanded.el-icon-caret-right:before {
            color: #4F8BE2 !important;
            font-weight: 600;
        }
        //没有子节点 小三角
        .el-tree /deep/ .el-tree-node__expand-icon.is-leaf::before {
            content: "";
            display: block;
            font-weight: 600;
            width: vw(12);
            height: vw(12);
            font-size: vw(16);
            background-size: 100% 100%;
        }

        > > > .el-tree-node__content .custom-tree-node {
            font-family: 'pfm';
            font-size: vw(16);
            color: #4e4e4e;
            line-height: vh(52);
            font-weight: 500;
            user-select: none;
        }

        > > > .el-tree-node__children .el-tree-node .el-tree-node__content .custom-tree-node {
            font-family: 'pf';
            font-size: vw(16);
            color: #666666;
            line-height: vh(52);
            font-weight: 500;
            user-select: none;
        }

        > > > .el-tree-node__content {
            border-left: vw(4) solid transparent;
        }
        //选中颜色
        /deep/ .el-tree--highlight-current .el-tree-node.is-current > .el-tree-node__content {
            background: #ECF4FF !important;
            border-left: vw(4) solid #4F8BE2;

            span {
                color: #4F8BE2;
                font-weight: 600;
            }

            .father {
                background: url('../assets/img/flower-xz.png') no-repeat;
                width: vw(18);
                height: vw(18);
                display: inline-block;
                background-size: 100% 100%;
                // margin-left: vw(20) !important;
                margin: vh(-3) vw(8) 0 vw(20) !important;
            }

            .son {
                width: vw(4);
                height: vw(4);
                border-radius: 50%;
                background: #4F8BE2;
                display: inline-block;
                background-size: 100% 100%;
                margin: 0 vw(8) vh(3) vw(35) !important;
            }
        }
    }
</style>